using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverworldController : OverworldController, IDataPersistence
{
    [SerializeField] bool isDead = false;

    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 3f;

    [SerializeField] BattleInfo infoForBattleSystem;

    int currentWaypointIndex = 0;
    // float timeSinceLastSawPlayer = Mathf.Infinity;
    // float timeSinceArrivedAtWaypoint = Mathf.Infinity;

    public override void Start()
    {
        base.Start();

        // if (enemyId == "")
        // {
        //     print("Enemy ID not set, generating now");
        //     GenerateGuid();
        // }
    }

    private void Update()
    {
        MoveEnemy();
        //UpdateTimers();
        SetAnimationState();
        flipCharacter.HandleFlip(OverworldInputManager.Instance.MovementInput);
    }

    private void MoveEnemy()
    {
        // if (timeSinceArrivedAtWaypoint > waypointDwellTime)
        // {
        //     // movementInput.x = nextPosition.x; CHANGE HOW THIS WORKS TO SMALLER INCREMENTS (normalize the vector)
        //     // movementInput.y = nextPosition.z;

        //     moveCommand.Execute(this);
        // }
        if (AtWaypoint())
        {
            CycleWaypoint();
        }

        DirectionTowardWaypoint();

        moveCommand.Execute(this);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 adjustedWaypointPosition = new Vector2(GetCurrentWaypoint().x, GetCurrentWaypoint().z);
        float distanceToWaypoint = Vector2.Distance(adjustedPlayerPosition, adjustedWaypointPosition);

        return distanceToWaypoint < waypointTolerance;
    }

    private void DirectionTowardWaypoint()
    {
        Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 adjustedWaypointPosition = new Vector2(GetCurrentWaypoint().x, GetCurrentWaypoint().z);
        Vector2 direction = adjustedWaypointPosition - adjustedPlayerPosition;

        //MovementInput = Vector2.ClampMagnitude(direction, 1);
    }


    private void UpdateTimers()
    {
        // timeSinceLastSawPlayer += Time.deltaTime;
        // timeSinceArrivedAtWaypoint += Time.deltaTime;
    }

    private void StartBattle()
    {
        BattleSettings battleSettings = GameObject.FindWithTag("BattleSettings").GetComponent<BattleSettings>();
        SceneSwitcher sceneSwitcher = GameObject.FindWithTag("SceneSwitcher").GetComponent<SceneSwitcher>();

        //battleInfoProxy.PreviousOverworldScene = "Tutorial"; //HAVE A MANAGER, not the enemy SET BATTLEINFOPROXY SOMEWHERE
        battleSettings.BattleInfo = infoForBattleSystem;
        sceneSwitcher.TransitionToBattle();
    }

    private void SetAnimationState()
    {
        if (IsCharacterMoving())
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
        //string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        //if (ES3.KeyExists(uniqueIdentifier))
        //{
        //    isDead = ES3.Load(uniqueIdentifier, false);
        //}

        //if (isDead)
        //{
        //    gameObject.SetActive(false);
        //    //set gameobject to inactive, print log, do something
        //}
    }

    public void SaveData()
    {
        //string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        //ES3.Save(uniqueIdentifier, isDead);
    }

}
