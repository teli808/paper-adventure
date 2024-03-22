using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    PlayerStatsManager statsManager;
    private void Start()
    {
        statsManager = PlayerStatsManager.Instance;
        SetPlayerCurrentHP();
        SetPlayerMaxHP();
        SetPlayerDamage();
        SetPlayerDefense();
    }
    public override void TakeDamage(int damage, float attackMultiplier, bool isTimedAttack)
    {
        int damageTaken;

        if (isTimedAttack)
        {
            damageTaken = Mathf.CeilToInt((damage * 2 - Defense) * 1 / attackMultiplier);
        }
        else
        {
            damageTaken = Mathf.CeilToInt((damage * 2 - Defense));
        }

        StartCoroutine(damageNumbers.PlayDamageAnimation(damageTaken));
        ChangeCurrentHP(damageTaken);
        StartCoroutine(damageQuality.PlayDamageQualityAnimation(isTimedAttack));
    }

    private void SetPlayerCurrentHP()
    {
        CurrentHP = statsManager.GetCurrentStat(Stat.HEALTH);
    }

    private void SetPlayerMaxHP()
    {
        MaxHP = statsManager.GetBaseStatByLevel(Stat.HEALTH, statsManager.CurrentLevel);
    }

    private void SetPlayerDamage()
    {
        Damage = statsManager.CurrentLevel * Damage;
    }

    private void SetPlayerDefense()
    {
        Defense = statsManager.CurrentLevel * Defense;
    }
}
