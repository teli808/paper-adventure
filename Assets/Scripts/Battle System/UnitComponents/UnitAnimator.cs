using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAnimator : MonoBehaviour
{
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator AttackedAnimation()
    {
        animator.SetBool("Attacked", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attacked", false);
    }

    public abstract IEnumerator TimedCorrectlyAnimation();

    public void DeathAnimation()
    {
        animator.SetBool("Dead", true);
    }
}
