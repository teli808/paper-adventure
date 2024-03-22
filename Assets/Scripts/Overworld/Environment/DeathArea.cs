using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] RespawnPoint respawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EventManager.Instance.OverworldDeath(FindNearestRespawnPoint());
        }
    }

    private Transform FindNearestRespawnPoint()
    {
        return respawnPoint.transform;
    }
}
