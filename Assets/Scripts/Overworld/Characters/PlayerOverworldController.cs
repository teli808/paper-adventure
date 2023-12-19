using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    IDLING,
    RUNNING,
    JUMPING,
    FALLING,
    TALKING,
    DISABLED
}

[RequireComponent(typeof(SaveId))]
public class PlayerOverworldController : OverworldController, IDataPersistence
{
    [SerializeField] PlayerInteractionHandler playerInteractionHandler;
    [SerializeField] LayerMask terrainLayer;

    [SerializeField] PauseMenu pauseMenu;

    [SerializeField] protected float overlapBoxDistToGround = -0.03f;
    [SerializeField] protected float overlapBoxScaleSizeX = 0.5f;
    [SerializeField] protected float overlapBoxScaleSizeY = 0.1f;
    [SerializeField] protected float overlapBoxScaleSizeZ = 0.45f;

    Command jumpCommand;
    Vector3 surfacePosition;
    //bool isGrounded = true;

    PlayerState playerState = PlayerState.IDLING;

    bool onPauseMenu = false;

    public override void Awake()
    {
        base.Awake();
        jumpCommand = GetComponent<Jump>();
        playerInteractionHandler = GetComponent<PlayerInteractionHandler>();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnChangePlayerState += ChangePlayerState;
        EventManager.Instance.OnDialogueOver += PlayerOnDialogueOver;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnChangePlayerState -= ChangePlayerState;
        EventManager.Instance.OnDialogueOver -= PlayerOnDialogueOver;
    }

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (onPauseMenu) return; //also check for inventory

        switch (playerState)
        {
            case PlayerState.IDLING:
                CheckPauseMenuInteraction();
                //CheckInventoryInteraction();

                if (!CheckGrounding())
                {
                    playerState = PlayerState.FALLING;

                    //isGrounded = false;
                }
                else if (IsCharacterMoving())
                {
                    playerState = PlayerState.RUNNING;
                }
                else if (OverworldInputManager.Instance.JumpInput)// && isGrounded)
                {
                    playerState = PlayerState.JUMPING;
                }
                else if (OverworldInputManager.Instance.GetInteractInput() && playerInteractionHandler.IsTouchingInteractable)
                {
                    playerInteractionHandler.CurrentInteractableObject.HandleUserInteraction();

                    if (playerInteractionHandler.CurrentInteractableObject.gameObject.GetComponent<InteractableDialogueTrigger>())
                    {
                        playerState = PlayerState.TALKING;
                    }
                    //else
                }

                break;
            case PlayerState.RUNNING:
                CheckPauseMenuInteraction();

                if (!CheckGrounding())
                {
                    playerState = PlayerState.FALLING;

                    //isGrounded = false;
                }
                else if (!IsCharacterMoving())
                {
                    playerState = PlayerState.IDLING;
                }
                else if (OverworldInputManager.Instance.JumpInput)// && isGrounded)
                {
                    playerState = PlayerState.JUMPING;
                }

                FlipCharacter();
                break;
            case PlayerState.JUMPING:
                CheckPauseMenuInteraction();

                if (MyRigidbody.velocity.y < 0f)
                {
                    playerState = PlayerState.FALLING;
                }
                
                FlipCharacter();
                break;
            case PlayerState.FALLING:
                CheckPauseMenuInteraction();

                if (CheckGrounding())
                {
                    if (MyRigidbody.velocity.y < 0f)
                    {
                        transform.position = new Vector3(transform.position.x, surfacePosition.y, transform.position.z); //Snap position to ground
                        MyRigidbody.velocity = new Vector3(MyRigidbody.velocity.x, 0f, MyRigidbody.velocity.z); //Y velocity to 0 to prevent jump hold issues
                    }

                    //isGrounded = true;

                    playerState = PlayerState.IDLING;
                    OverworldInputManager.Instance.EnableJump();
                }

                FlipCharacter();
                break;
            case PlayerState.TALKING:
                if (OverworldInputManager.Instance.GetInteractInput())
                {
                    EventManager.Instance.DialogueSubmitted();
                }
                break;
            case PlayerState.DISABLED:
                break;
            default:
                break;
        }

        SetAnimationState();
    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.RUNNING:
                moveCommand.Execute(this);
                break;
            case PlayerState.JUMPING:
                if (OverworldInputManager.Instance.JumpInput)
                {
                    jumpCommand.Execute(this);
                    //isGrounded = false;

                    OverworldInputManager.Instance.DisableJump();
                    OverworldInputManager.Instance.JumpInput = false;
                }
                moveCommand.Execute(this);
                break;
            case PlayerState.FALLING:
                moveCommand.Execute(this);
                break;
            default:
                break;
        }
    }

    private void FlipCharacter()
    {
        flipCharacter.HandleFlip(OverworldInputManager.Instance.MovementInput);
    }

    private void ChangePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;

        if (playerState == PlayerState.IDLING) OverworldInputManager.Instance.EnableOverworldActionMap();

        //print("Player state = " + this.playerState);
    }

    private void PlayerOnDialogueOver() //fix in cutscenehandler
    {
        if (playerState == PlayerState.DISABLED) return; //Handles race conditions, cutscene could trigger first in which playerstate will disable
        ChangePlayerState(PlayerState.IDLING);
    }

    private void CheckPauseMenuInteraction()
    {
        if (OverworldInputManager.Instance.GetPauseMenuInput() && !onPauseMenu)
        {
            OverworldInputManager.Instance.DisableOverworldActionMap();
            onPauseMenu = true;
            pauseMenu.ShowMenu();
        }
    }

    public void ResumeFromPauseMenu()
    {
        OverworldInputManager.Instance.EnableOverworldActionMap();
        onPauseMenu = false;
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
            surfacePosition = hitColliders[0].ClosestPoint(transform.position);
            return true;
        }
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

    private void SetAnimationState()
    {
        switch (playerState)
        {
            case PlayerState.IDLING:
            case PlayerState.TALKING:
                animator.SetBool("Jumping", false);
                animator.SetBool("Running", false);
                break;
            case PlayerState.RUNNING:
                animator.SetBool("Jumping", false);
                animator.SetBool("Running", true);
                break;
            case PlayerState.JUMPING:
                animator.SetBool("Jumping", true);
                break;
            //case PlayerState.FALLING:
            //    animator.SetBool("Jumping", true);
            //    break;
            default:
                break;
        }
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "transformPosition")))
        {
            transform.position = ES3.Load<Vector3>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "transformPosition"));
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "transformPosition"), transform.position);
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
