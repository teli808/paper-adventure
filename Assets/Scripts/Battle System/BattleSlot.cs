using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSlot : MonoBehaviour
{
    public Unit CurrentUnit { get; private set; }

    public int SlotNumber { get; set; }

    public bool HasUnit()
    {
        return (CurrentUnit != null);
    }

    public void InstantiateUnit(Unit unitToInstantiate)
    {
        //if unit is flying...

        //else
        CurrentUnit = Instantiate(unitToInstantiate, transform.position, Quaternion.identity);

        CurrentUnit.gameObject.name = CurrentUnit.UnitName;
        CurrentUnit.CurrentSlotNumber = SlotNumber;
    }

    public IEnumerator DestroyUnit()
    {
        CurrentUnit.DeathAnimation();
        //award exp, etc
        yield return new WaitForSeconds(1f);
        Destroy(CurrentUnit.gameObject);
        CurrentUnit = null;
    }
}
