using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum NPCState
{
    IDLING,
    WALKING,
}

public class NPCController : OverworldController
{
    Animator animator;

    NPCState npcState = NPCState.IDLING;

    [SerializeField] WaypointBehavior waypointBehavior;
    [SerializeField] bool shouldMove = false;

    Command idleCommand;
    Command disableCommand;

    // Add behavior: What happens if currently talking?

    public override void Awake()
    {
        base.Awake();

        idleCommand = GetComponent<Idle>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (npcState)
        {
            case NPCState.IDLING:
                idleCommand.Execute(this);

                if (!shouldMove || !waypointBehavior.ShouldMove()) break;

                if (waypointBehavior.CheckAtWaypoint())
                {
                    MovementInput = Vector2.zero;
                    waypointBehavior.HandleAtWaypoint();
                    break;
                }
                else
                {
                    npcState = NPCState.WALKING;
                }
                break;
            case NPCState.WALKING:
                MovementInput = waypointBehavior.ChangeDirectionTowardWaypoint();

                moveCommand.Execute(this);

                if (waypointBehavior.CheckAtWaypoint())
                {
                    MovementInput = Vector2.zero;
                    waypointBehavior.HandleAtWaypoint();
                    npcState = NPCState.IDLING;
                    break;
                }
                break;
            //case NPCState.DISABLED:
            //    disableCommand.Execute(this);
            //    break;
        }

        if (shouldMove) waypointBehavior.UpdateWaypointTimer();

        SetAnimationState();
        flipCharacter.HandleFlip(MovementInput);
    }

    public override bool CheckMoveInput()
    {
        bool enemyHasHorizontalSpeed = !Mathf.Approximately(MovementInput.x, Mathf.Epsilon);
        bool enemyHasForwardSpeed = !Mathf.Approximately(MovementInput.y, Mathf.Epsilon);

        return enemyHasHorizontalSpeed || enemyHasForwardSpeed;
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

}
