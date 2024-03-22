using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    IDLING,
    RUNNING,
    JUMPING,
    FALLING,
    TALKING_TO_NPC,
    TALKING_IN_CUTSCENE,
    DEAD,
    DISABLED,
}

[RequireComponent(typeof(SaveId))]
public class PlayerOverworldController : OverworldController, IDataPersistence
{
    PlayerInteractionHandler playerInteractionHandler;
    [SerializeField] LayerMask terrainLayer;

    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] PlayerManagementDisplay playerManagementDisplay;

    [SerializeField] protected float overlapBoxDistToGround = -0.03f;
    [SerializeField] protected float overlapBoxScaleSizeX = 0.5f;
    [SerializeField] protected float overlapBoxScaleSizeY = 0.1f;
    [SerializeField] protected float overlapBoxScaleSizeZ = 0.45f;

    PlayerAnimation playerAnimation;

    Command idleCommand;
    Command jumpCommand;
    Command fallCommand;
    Command disableCommand;

    Vector3 surfacePosition;
    //bool isGrounded = true;

    PlayerState playerState = PlayerState.DISABLED;

    bool onPauseMenu = false;
    bool onPlayerManagementMenu = false;

    public override void Awake()
    {
        base.Awake();

        jumpCommand = GetComponent<Jump>();
        fallCommand = GetComponent<Falling>();
        idleCommand = GetComponent<Idle>();
        disableCommand = GetComponent<Disable>();

        playerInteractionHandler = GetComponent<PlayerInteractionHandler>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnChangePlayerState += ChangePlayerState;
        //EventManager.Instance.OnDialogueOver += PlayerOnDialogueOver;
        EventManager.Instance.OnSpawnPlayerAtPosition += SpawnPlayerAtPosition;
        EventManager.Instance.OnFaderComplete += OnFadeComplete;
        EventManager.Instance.OnOverworldDeath += OnDeathEvent;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnChangePlayerState -= ChangePlayerState;
        //EventManager.Instance.OnDialogueOver -= PlayerOnDialogueOver;
        EventManager.Instance.OnSpawnPlayerAtPosition -= SpawnPlayerAtPosition;
        EventManager.Instance.OnFaderComplete -= OnFadeComplete;
        EventManager.Instance.OnOverworldDeath -= OnDeathEvent;
    }

    void Update()
    {
        //print(playerState);

        if (onPauseMenu) return;

        if (onPlayerManagementMenu)
        {
            CheckPlayerManagementCanceledInteraction();
            return;
        }

        switch (playerState)
        {
            case PlayerState.IDLING:
                CheckPauseMenuInteraction();
                CheckPlayerManagementMenuInteraction();

                if (!CheckGrounding())
                {
                    playerState = PlayerState.FALLING;
                }
                else if (CheckMoveInput())
                {
                    playerState = PlayerState.RUNNING;
                }
                else if (OverworldInputManager.Instance.JumpInput)
                {
                    playerState = PlayerState.JUMPING;
                }
                else if (OverworldInputManager.Instance.GetInteractInput() && playerInteractionHandler.IsTouchingInteractable)
                {
                    playerInteractionHandler.CurrentInteractableObject.HandleUserInteraction();

                    if (playerInteractionHandler.CurrentInteractableObject.gameObject.GetComponent<InteractableDialogue>())
                    {
                        playerState = PlayerState.TALKING_TO_NPC;
                    }
                }

                break;
            case PlayerState.RUNNING:
                CheckPauseMenuInteraction();
                CheckPlayerManagementMenuInteraction();

                if (!CheckGrounding())
                {
                    playerState = PlayerState.FALLING;

                    //isGrounded = false;
                }
                else if (!CheckMoveInput())
                {
                    playerState = PlayerState.IDLING;
                }
                else if (OverworldInputManager.Instance.JumpInput)
                {
                    playerState = PlayerState.JUMPING;
                }

                FlipCharacter();
                break;
            case PlayerState.JUMPING:
                CheckPauseMenuInteraction();
                CheckPlayerManagementMenuInteraction();

                if (MyRigidbody.velocity.y < 0f)
                {
                    playerState = PlayerState.FALLING;
                }
                
                FlipCharacter();
                break;
            case PlayerState.FALLING:
                CheckPauseMenuInteraction();
                CheckPlayerManagementMenuInteraction();

                if (CheckGrounding())
                {
                    if (MyRigidbody.velocity.y < 0f)
                    {
                        transform.position = new Vector3(transform.position.x, surfacePosition.y + GetHeightOfCharacter() / 2, transform.position.z);
                        // transform.position = new Vector3(transform.position.x, surfacePosition.y, transform.position.z); unneeded
                        MyRigidbody.velocity = new Vector3(MyRigidbody.velocity.x, 0f, MyRigidbody.velocity.z); //Y velocity to 0 to prevent jump hold issues
                    }

                    playerState = PlayerState.IDLING;
                    OverworldInputManager.Instance.EnableJump();
                }

                FlipCharacter();
                break;
            case PlayerState.TALKING_TO_NPC:
                if (OverworldInputManager.Instance.GetInteractInput())
                {
                    EventManager.Instance.DialogueSubmitted();
                }
                break;
            case PlayerState.TALKING_IN_CUTSCENE:
                if (OverworldInputManager.Instance.GetInteractInput())
                {
                    EventManager.Instance.DialogueSubmitted();
                }
                break;
            default:
                break;
        }

        playerAnimation.SetAnimationState(playerState);
    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.IDLING:
                idleCommand.Execute(this);
                break;
            case PlayerState.RUNNING:
                MovementInput = OverworldInputManager.Instance.MovementInput;
                moveCommand.Execute(this);
                break;
            case PlayerState.JUMPING:
                if (OverworldInputManager.Instance.JumpInput)
                {
                    jumpCommand.Execute(this);

                    OverworldInputManager.Instance.DisableJump();
                    OverworldInputManager.Instance.JumpInput = false;
                }
                MovementInput = OverworldInputManager.Instance.MovementInput;
                moveCommand.Execute(this);
                break;
            case PlayerState.FALLING:
                fallCommand.Execute(this);
                break;
            case PlayerState.DISABLED:
                disableCommand.Execute(this);
                break;
            default:
                break;
        }
    }

    private void SpawnPlayerAtPosition(Transform transform)
    {
        this.transform.position = transform.position;

        print("Spawned player at position from SpawnPlayerAtPosition to: " + this.transform.position);
    }

    private void FlipCharacter()
    {
        flipCharacter.HandleFlip(OverworldInputManager.Instance.MovementInput);
    }

    public override bool CheckMoveInput()
    {
        bool playerHasHorizontalSpeed = !Mathf.Approximately(OverworldInputManager.Instance.MovementInput.x, Mathf.Epsilon);
        bool playerHasForwardSpeed = !Mathf.Approximately(OverworldInputManager.Instance.MovementInput.y, Mathf.Epsilon);

        return playerHasHorizontalSpeed || playerHasForwardSpeed;
    }

    private void ChangePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;

        if (playerState == PlayerState.IDLING) OverworldInputManager.Instance.EnableGameplayActionMap();
    }

    //private void PlayerOnDialogueOver() //fix in cutscenehandler
    //{
    //    if (playerState == PlayerState.DISABLED) return; //Handles race conditions, cutscene could trigger first in which playerstate will disable
    //    ChangePlayerState(PlayerState.IDLING);
    //}

    private void OnFadeComplete()
    {
        ChangePlayerState(PlayerState.IDLING);
    }

    private void OnDeathEvent(Transform respawnPoint)
    {
        StartCoroutine(DeathTransition(respawnPoint));
    }

    private IEnumerator DeathTransition(Transform respawnPoint)
    {
        ChangePlayerState(PlayerState.DEAD);
        //do we still have to set other animator bools to false or will update get called?

        playerAnimation.StartDeathAnimation();

        yield return new WaitUntil(() => playerAnimation.DeathAnimationDone);

        playerAnimation.DeathAnimationDone = false;
 
        transform.position = respawnPoint.position;

        ChangePlayerState(PlayerState.IDLING);
    }

    private void CheckPauseMenuInteraction()
    {
        if (OverworldInputManager.Instance.GetPauseMenuInput() && !onPauseMenu)
        {
            AudioManager.Instance.PauseAllSFX();
            OverworldInputManager.Instance.SwitchToInMenuMap();
            onPauseMenu = true;
            pauseMenu.ShowMenu();
        }
    }

    public void ResumeFromPauseMenu()
    {
        AudioManager.Instance.UnpauseAllSFX();
        OverworldInputManager.Instance.SwitchToGameplayActionMap();
        onPauseMenu = false;
    }

    private void CheckPlayerManagementMenuInteraction()
    {
        if (OverworldInputManager.Instance.GetInventoryMenuInput() && !onPlayerManagementMenu)
        {
            OverworldInputManager.Instance.SwitchToInMenuMap();
            onPlayerManagementMenu = true;
            playerManagementDisplay.OpenInventoryDisplay();
        }
        else if (OverworldInputManager.Instance.GetStatsMenuInput() && !onPlayerManagementMenu)
        {
            OverworldInputManager.Instance.SwitchToInMenuMap();
            onPlayerManagementMenu = true;
            playerManagementDisplay.OpenStatsDisplay();
        }
    }

    private void CheckPlayerManagementCanceledInteraction()
    {
        if (OverworldInputManager.Instance.GetCancelPlayerManagementInput() && onPlayerManagementMenu)
        {
            OverworldInputManager.Instance.SwitchToGameplayActionMap();
            onPlayerManagementMenu = false;
            playerManagementDisplay.ClosePlayerManagementDisplay();
        }
    }

    public bool CheckGrounding()
    {
        Vector3 point = transform.localPosition + Vector3.down * overlapBoxDistToGround;
        Vector3 size = new Vector3(transform.localScale.x * overlapBoxScaleSizeX, transform.localScale.y * overlapBoxScaleSizeY, transform.localScale.z * overlapBoxScaleSizeZ);

        Collider[] hitColliders = Physics.OverlapBox(point, size, Quaternion.identity, terrainLayer);

        if (hitColliders.Length == 0)
        {
            return false;
        }
        else
        {
            Vector3 adjustedPosition = new Vector3(transform.position.x, transform.position.y - GetHeightOfCharacter() / 2, transform.position.z); //new
            //surfacePosition = hitColliders[0].ClosestPoint(transform.position);
            surfacePosition = hitColliders[0].ClosestPoint(adjustedPosition);
            return true;
        }
    }

    private float GetHeightOfCharacter()
    {
        return GetComponent<CapsuleCollider>().height;
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(groundPoint.position - groundPoint.up * distanceToGround, raySphereRadius);
        Vector3 point = transform.localPosition + Vector3.down * overlapBoxDistToGround;
        Vector3 size = new Vector3(transform.localScale.x * overlapBoxScaleSizeX, transform.localScale.y * overlapBoxScaleSizeY, transform.localScale.z * overlapBoxScaleSizeZ);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(point, size);
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "transformPosition")) 
            && ES3.Load<string>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameOfPosition")) == SceneManager.GetActiveScene().name)
        {
            transform.position = ES3.Load<Vector3>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "transformPosition"));

            print("Position just changed from LoadData to: " + transform.position);
        }
        else
        {
            print("Either player's transformPosition was not previously saved or loaded into a scene that was not previously saved");
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "transformPosition"), transform.position);
        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameOfPosition"), SceneManager.GetActiveScene().name);
    }

    // private void CheckGroundingSphereCast()
    // {
    //     RaycastHit hit;

    //     if (Physics.SphereCast(groundPoint.position, raySphereRadius, Vector3.down, out hit, distanceToGround, terrainLayer))
    //     {
    //         isGrounded = true;
    //     }
    //     else
    //     {
    //         isGrounded = false;
    //     }
    // }


}
