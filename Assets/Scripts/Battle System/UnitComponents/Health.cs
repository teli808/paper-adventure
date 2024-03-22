using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [field: SerializeField] public int MaxHP { get; protected set; } = 10;
    [field: SerializeField] public int CurrentHP { get; protected set; } = 10;
    [field: SerializeField] public int Damage { get; protected set; } = 1;
    [field: SerializeField] public int Defense { get; protected set; } = 1;

    protected DamageNumbers damageNumbers;
    protected DamageQuality damageQuality;

    private void Awake()
    {
        damageNumbers = GetComponentInChildren<DamageNumbers>();
        damageQuality = GetComponentInChildren<DamageQuality>();
    }

    public abstract void TakeDamage(int damage, float attackMultiplier, bool isTimedAttack);

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

    public void ChangeCurrentHP(int damageTaken)
    {
        float newHP = CurrentHP - damageTaken;
        CurrentHP = (int) Mathf.Clamp(newHP, 0, MaxHP);
    }
}
