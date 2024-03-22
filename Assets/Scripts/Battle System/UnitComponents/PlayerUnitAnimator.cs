using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitAnimator : UnitAnimator
{
    public override IEnumerator TimedCorrectlyAnimation()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Battle.BlockedCorrectly, transform.position);

        animator.ResetTrigger("Blocked");
        animator.SetTrigger("Blocked");
        yield return null;
    }
}
