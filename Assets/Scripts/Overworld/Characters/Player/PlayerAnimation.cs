using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    public bool DeathAnimationDone { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.IDLING:
            case PlayerState.TALKING_TO_NPC:
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
            case PlayerState.DISABLED:
                animator.SetBool("Jumping", false);
                animator.SetBool("Running", false);
                break;
            default:
                break;
        }
    }

    public void StartDeathAnimation()
    {
        animator.SetBool("Jumping", false);
        animator.SetBool("Running", false);

        //animator.ResetTrigger("Dead");
        animator.SetTrigger("Dead");
    }

    public void DeathAnimationFinished()
    {
        DeathAnimationDone = true;

        
    }
}
