using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitLevel : UnitLevel
{
    void Start()
    {
        SetPlayerLevel();
    }

    private void SetPlayerLevel()
    {
        Level = PlayerStatsManager.Instance.CurrentLevel;
    }
}
