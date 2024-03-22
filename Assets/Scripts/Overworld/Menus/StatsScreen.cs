using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsScreen : PlayerManagementScreen
{
    [SerializeField] TextMeshProUGUI levelValue;
    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] TextMeshProUGUI energyValue;
    [SerializeField] TextMeshProUGUI experienceValue;

    private void SetUpScreen()
    {
        int currentLevel = PlayerStatsManager.Instance.CurrentLevel;

        levelValue.text = currentLevel.ToString();
        hpValue.text = PlayerStatsManager.Instance.GetCurrentStat(Stat.HEALTH).ToString() + " / " 
            + PlayerStatsManager.Instance.GetBaseStatByLevel(Stat.HEALTH, currentLevel).ToString();
        energyValue.text = PlayerStatsManager.Instance.GetCurrentStat(Stat.ENERGY).ToString() + " / "
            + PlayerStatsManager.Instance.GetBaseStatByLevel(Stat.ENERGY, currentLevel).ToString();
        experienceValue.text = PlayerStatsManager.Instance.GetCurrentStat(Stat.EXPERIENCE).ToString() + " / "
            + PlayerStatsManager.Instance.GetBaseStatByLevel(Stat.EXPERIENCE, currentLevel).ToString();
    }

    public override void ShowScreen()
    {
        SetUpScreen();

        base.ShowScreen();

        topBar.StatsScreenOpened();
    }
}
