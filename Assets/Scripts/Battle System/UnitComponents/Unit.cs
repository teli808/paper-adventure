using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    PLAYER,
    ENEMY,
}

public enum SpecialType
{
    NONE,
    FLYING,
}

public class Unit : MonoBehaviour
{
    [field: SerializeField] public TimingCollider TimingCollider { get; private set; }
    [field: SerializeField] public FittedBattleCollider FittedBattleCollider { get; private set; }
    [field: SerializeField] public string UnitName { get; private set; } = "DefaultName";
    [field: SerializeField] public SpecialType SpecialType { get; private set; } = SpecialType.NONE;
    [field: SerializeField] public float EXPMultiplier { get; private set; } = 1.0f; //unneeded for player
    public UnitAnimator UnitAnimator { get; private set; }

    Health health;
    UnitLevel unitLevel;

    public int CurrentSlotNumber { get; set; }
    private void Awake()
    {
        UnitAnimator = GetComponent<UnitAnimator>();
        health = GetComponent<Health>();
        unitLevel = GetComponent<UnitLevel>();

        print(unitLevel.GetType().Name);

    }

    public void TakeDamage(int damage, float multiplier, bool isTimedAttack)
    {
        health.TakeDamage(damage, multiplier, isTimedAttack);
    }

    public void SetTimingColliderSettings(Collider colliderEnteringZone)
    {
        TimingCollider.SetColliderSettings(colliderEnteringZone);
    }

    public void SetFittedBattleColliderSettings(Collider colliderEnteringZone)
    {
        FittedBattleCollider.SetColliderSettings(colliderEnteringZone);
    }

    public bool CheckIsDead()
    {
        return health.IsDead();
    }

    public int GetCurrentHP()
    {
        return health.CurrentHP;
    }

    public int GetAttackDamage()
    {
        return health.Damage;
    }

    public int GetLevel()
    {
        return unitLevel.Level;
    }

}
