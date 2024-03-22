using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 3f;

    float timeSinceArrivedAtWaypoint = Mathf.Infinity;

    int currentWaypointIndex = 0;

    public Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    public void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    public bool CheckAtWaypoint()
    {
        Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 adjustedWaypointPosition = new Vector2(GetCurrentWaypoint().x, GetCurrentWaypoint().z);
        float distanceToWaypoint = Vector2.Distance(adjustedPlayerPosition, adjustedWaypointPosition);

        return distanceToWaypoint < waypointTolerance;
    }

    public void HandleAtWaypoint()
    {
        timeSinceArrivedAtWaypoint = 0f;
        CycleWaypoint();
    }

    public Vector2 ChangeDirectionTowardWaypoint()
    {
        Vector2 adjustedPlayerPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 adjustedWaypointPosition = new Vector2(GetCurrentWaypoint().x, GetCurrentWaypoint().z);
        Vector2 direction = adjustedWaypointPosition - adjustedPlayerPosition;

        Vector2 movementInput = Vector2.ClampMagnitude(direction, 1);

        return movementInput;
    }

    public bool ShouldMove()
    {
        return timeSinceArrivedAtWaypoint > waypointDwellTime;
    }

    public void UpdateWaypointTimer()
    {
        timeSinceArrivedAtWaypoint += Time.deltaTime;
    }
}
