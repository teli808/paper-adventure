using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float CurrentHP { get; private set; }
    [SerializeField] UnitHealthBar unitHealthBar;

    DamageNumbers damageNumbers;
    DamageQuality damageQuality;

    private void Awake()
    {
        damageNumbers = GetComponentInChildren<DamageNumbers>();
        damageQuality = GetComponentInChildren<DamageQuality>();
    }

    public void TakeDamage(float damage, float multiplier, bool isTimedAttack) //refactor
    {
        float previousHP = CurrentHP;

        if (isTimedAttack)
        {
            StartCoroutine(damageNumbers.PlayDamageAnimation(damage * multiplier));
            ChangeCurrentHP(damage, multiplier);
        }
        else
        {
            StartCoroutine(damageNumbers.PlayDamageAnimation(damage));
            ChangeCurrentHP(damage, 1f);
        }

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

    public bool CheckHPBelowPercentage(float decimalPercentage)
    {
        return CurrentHP / MaxHP < decimalPercentage;
    }

    public bool IsDead()
    {
        if (CurrentHP > 0)
        {
            return false;
        }

        return true;
    }

    public void ChangeCurrentHP(float damage, float multiplier)
    {
        float newHP = CurrentHP - (damage * multiplier);
        CurrentHP = Mathf.Clamp(newHP, 0, MaxHP);
    }
}
