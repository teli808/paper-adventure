using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollider : MonoBehaviour
{
    Collider currentColliderEnteringZone;

    public bool CurrentlyTouching { get; private set; } = false;

    private void OnTriggerStay(Collider other)
    {
        if (other == currentColliderEnteringZone)
        {
            CurrentlyTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentColliderEnteringZone)
        {
            CurrentlyTouching = false;
        }
    }
    public void SetColliderSettings(Collider colliderEnteringZone)
    {
        currentColliderEnteringZone = colliderEnteringZone;
    }

    public void ResetSettings()
    {
        CurrentlyTouching = false;
        currentColliderEnteringZone = null;
    }

    public Collider GetUnitCollider()
    {
        return GetComponent<Collider>();
    }
}
