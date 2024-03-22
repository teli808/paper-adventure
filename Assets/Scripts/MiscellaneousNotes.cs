using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousNotes : MonoBehaviour
{
    // private void OnDrawGizmos()
    // {
    //     Vector3 point = transform.localPosition + Vector3.down * overlapBoxDistToGround;
    //     Vector3 size = new Vector3(transform.localScale.x * overlapBoxScaleSizeX, transform.localScale.y * overlapBoxScaleSizeY, transform.localScale.z * overlapBoxScaleSizeZ);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(point, size);
    // }


    // public bool JumpCheckAboveEnemy(GameObject targetUnit)
    // {
    //     Vector3 point = transform.localPosition + Vector3.down * overlapBoxDistToGround;
    //     Vector3 size = new Vector3(transform.localScale.x * overlapBoxScaleSizeX, transform.localScale.y * overlapBoxScaleSizeY, transform.localScale.z * overlapBoxScaleSizeZ);

    //     Collider[] hitColliders = Physics.OverlapBox(point, size, Quaternion.identity, unitLayer);

    //     if (hitColliders.Length == 0) return false;

    //     foreach (Collider collider in hitColliders)
    //     {
    //         if (collider.gameObject == targetUnit)
    //         {
    //             return true;
    //         }
    //     }

    //     return false;
    // }
}
