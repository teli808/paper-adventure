using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitAnimator : UnitAnimator
{
    public override IEnumerator TimedCorrectlyAnimation() // How the enemy reacts when player times attack on them correctly
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Battle.TimedCorrectly, transform.position);

        yield return AttackedAnimation(); //add different animation later
    }
}
