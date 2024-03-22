using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : StatBar
{
    public override void UpdateCurrentStat()
    {
        string currentHP = BattleSlotManager.Instance.GetPlayerUnit().GetCurrentHP().ToString();

        if (currentStatComponent.text != currentHP)
        {
            //tween animation
            currentStatComponent.text = currentHP;
        }
    }

    public override void UpdateMaxStat()
    {
        maxStatComponent.text = BattleSlotManager.Instance.GetPlayerUnit().GetCurrentHP().ToString();
    }
}

