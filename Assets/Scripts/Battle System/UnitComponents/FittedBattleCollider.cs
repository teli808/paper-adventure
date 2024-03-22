using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FittedBattleCollider : UnitCollider
{
    public float GetCapsuleRadius()
    {
        float radius = GetComponent<CapsuleCollider>().radius;

        return radius;
    }

    public Vector3 GetCapsuleBoundsCenter()
    {
        Vector3 boundsCenter = GetComponent<CapsuleCollider>().bounds.center;

        return boundsCenter;
    }
}
