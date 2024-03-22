using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimingHandler : MonoBehaviour
{
    protected bool inputAllowed = false;
    protected bool playerPressedButton = false;
    public bool IsTimedCorrectly { get; private set; } = false; //maintain this after the attack as certain systems may need it

    Unit unitTarget;

    protected void CheckCorrectTiming()
    {
        if (unitTarget.TimingCollider.CurrentlyTouching)
        {
            print("Correct timing!");
            IsTimedCorrectly = true;
        }
        else
        {
            print("Incorrect timing!");
        }

        playerPressedButton = true;
    }

    public void EnableHandling()
    {
        inputAllowed = true;
    }

    public void DisableHandling()
    {
        inputAllowed = false;
    }

    public void SetUpTimingHandler(Unit unitTarget, Collider attackingCollider) //change to gameobject
    {
        this.unitTarget = unitTarget;

        unitTarget.FittedBattleCollider.ResetSettings();
        unitTarget.TimingCollider.ResetSettings();

        unitTarget.SetTimingColliderSettings(attackingCollider);
        unitTarget.SetFittedBattleColliderSettings(attackingCollider);
    }

    public abstract bool GetKeyPressed();

    public void ResetInputSettings()
    {
        DisableHandling();
        playerPressedButton = false;
        IsTimedCorrectly = false;
    }
}
