using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] UnitHealthBar unitHealthBar;
    public override void TakeDamage(int damage, float attackMultiplier, bool isTimedAttack)
    {
        int previousHP = CurrentHP;

        int damageTaken;

        if (isTimedAttack)
        {
            damageTaken = Mathf.CeilToInt((damage * 2 - Defense) * attackMultiplier);
        }
        else
        {
            damageTaken = Mathf.CeilToInt((damage * 2 - Defense));
        }

        StartCoroutine(damageNumbers.PlayDamageAnimation(damageTaken));
        ChangeCurrentHP(damageTaken);
        StartCoroutine(damageQuality.PlayDamageQualityAnimation(isTimedAttack));

        ManageHealthBar(previousHP);
    }

    private void ManageHealthBar(float previousHP)
    {
        if (unitHealthBar != null)
        {
            unitHealthBar.SetAttackedBefore();
            unitHealthBar.PlayHealthAnimation(CurrentHP, previousHP, MaxHP);
        }
    }
}
