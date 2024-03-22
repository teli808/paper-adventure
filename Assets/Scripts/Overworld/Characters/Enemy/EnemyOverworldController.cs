using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    IDLING,
    WALKING,
    CHASING,
    DISABLED
}

public class EnemyOverworldController : OverworldController, IDataPersistence
{
    [SerializeField] bool isDead = false;

    [SerializeField] float chaseDistance = 3f;
    [SerializeField] float suspicionTime = 3f;
    [SerializeField] EnemyChaseVisualCue enemyChaseVisualCue;

    //[SerializeField] PatrolPath patrolPath;
    //[SerializeField] float waypointTolerance = 1f;
    //[SerializeField] float waypointDwellTime = 3f;

    [SerializeField] WaypointBehavior waypointBehavior;
    [SerializeField] BattleInfo infoForBattleSystem;

    Animator animator;

    //int currentWaypointIndex = 0;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    //float timeSinceArrivedAtWaypoint = Mathf.Infinity;

    Command idleCommand;
    Command disableCommand;

    EnemyState enemyState = EnemyState.IDLING;

    public override void Awake()
    {
        base.Awake();

        idleCommand = GetComponent<Idle>();
        disableCommand = GetComponent<Disable>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.IDLING:
                idleCommand.Execute(this);

                if (InAttackRangeOfPlayer() && SuspicionTimeReached())
                {
                    StartCoroutine(enemyChaseVisualCue.DisplayCue());
                    enemyState = EnemyState.CHASING;
                }

                if (!waypointBehavior.ShouldMove()) break;

                if (waypointBehavior.CheckAtWaypoint())
                {
                    //timeSinceArrivedAtWaypoint = 0f;
                    MovementInput = Vector2.zero;
                    waypointBehavior.HandleAtWaypoint();
                    //CycleWaypoint();
                    break;
                }
                else
                {
                    enemyState = EnemyState.WALKING;
                }
                break;
            case EnemyState.WALKING:
                MovementInput = waypointBehavior.ChangeDirectionTowardWaypoint();

                moveCommand.Execute(this);

                if (InAttackRangeOfPlayer() && SuspicionTimeReached())
                {
                    StartCoroutine(enemyChaseVisualCue.DisplayCue());
                    enemyState = EnemyState.CHASING;
                }
                else if (waypointBehavior.CheckAtWaypoint())
                {
                    //timeSinceArrivedAtWaypoint = 0f;
                    MovementInput = Vector2.zero;
                    waypointBehavior.HandleAtWaypoint();
                    enemyState = EnemyState.IDLING;
                    //CycleWaypoint();
                    break;
                }
                break;
            case EnemyState.CHASING:
                if (InAttackRangeOfPlayer())
                {
                    ChaseBehavior();
                }
                else
                {
                    timeSinceLastSawPlayer = 0f;
                    enemyState = EnemyState.WALKING;
                }
                break;
            case EnemyState.DISABLED:
                disableCommand.Execute(this);
                break;

        }

        waypointBehavior.UpdateWaypointTimer();
        UpdateLastSawPlayerTimer();
        SetAnimationState();
        flipCharacter.HandleFlip(MovementInput);
    }

    public override bool CheckMoveInput()
    {
        bool enemyHasHorizontalSpeed = !Mathf.Approximately(MovementInput.x, Mathf.Epsilon);
        bool enemyHasForwardSpeed = !Mathf.Approximately(MovementInput.y, Mathf.Epsilon);

        return enemyHasHorizontalSpeed || enemyHasForwardSpeed;
    }

    //private Vector3 GetCurrentWaypoint()
    //{
    //    return patrolPath.GetWaypoint(currentWaypointIndex);
    //}

    //private void CycleWaypoint()
    //{
    //    currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    //}

    //private bool AtWaypoint()
    //{
    //    Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
    //    Vector2 adjustedWaypointPosition = new Vector2(GetCurrentWaypoint().x, GetCurrentWaypoint().z);
    //    float distanceToWaypoint = Vector2.Distance(adjustedPlayerPosition, adjustedWaypointPosition);

    //    return distanceToWaypoint < waypointTolerance;
    //}

    //private void ChangeDirectionTowardWaypoint()
    //{
    //    Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
    //    Vector2 adjustedWaypointPosition = new Vector2(GetCurrentWaypoint().x, GetCurrentWaypoint().z);
    //    Vector2 direction = adjustedWaypointPosition - adjustedPlayerPosition;

    //    MovementInput = Vector2.ClampMagnitude(direction, 1);
    //}

    private void ChaseBehavior() //need to increase speed
    {
        timeSinceLastSawPlayer = 0;

        Transform playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 adjustedWaypointPosition = new Vector2(playerTransform.position.x, playerTransform.position.z);
        Vector2 direction = adjustedWaypointPosition - adjustedPlayerPosition;

        MovementInput = Vector2.ClampMagnitude(direction, 1);

        moveCommand.Execute(this);

        //fighter.Attack(player);
    }

    private bool SuspicionTimeReached()
    {
        return timeSinceLastSawPlayer > suspicionTime;
    }

    private bool InAttackRangeOfPlayer()
    {
        Transform playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        return distanceToPlayer < chaseDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    //private void UpdateTimers()
    //{
    //    timeSinceLastSawPlayer += Time.deltaTime;
    //    //timeSinceArrivedAtWaypoint += Time.deltaTime;
    //}

    private void UpdateLastSawPlayerTimer()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    private void StartBattle()
    {
        BattleSettings battleSettings = GameObject.FindWithTag("BattleSettings").GetComponent<BattleSettings>();
        SceneSwitcher sceneSwitcher = GameObject.FindWithTag("SceneSwitcher").GetComponent<SceneSwitcher>();

        //Disable controls so player and enemies stop moving
        EventManager.Instance.ChangePlayerState(PlayerState.DISABLED);
        enemyState = EnemyState.DISABLED;

        isDead = true; //Game will save as battle starts

        battleSettings.BattleInfo = infoForBattleSystem;
        sceneSwitcher.TransitionToNormalBattle();
    }

    private void SetAnimationState()
    {
        if (CheckMoveInput())
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartBattle();
        }

    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "isDead")))
        {
            isDead = ES3.Load<bool>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "isDead"));
        }

        if (isDead)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "isDead"), isDead);
    }
}
