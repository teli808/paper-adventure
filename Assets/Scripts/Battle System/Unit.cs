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
    [field: SerializeField] public UnitType UnitType { get; private set; } = UnitType.ENEMY;

    [SerializeField] SpecialType specialType = SpecialType.NONE;
    [SerializeField] int unitLevel = 1;
    [field: SerializeField] public float Damage { get; private set; } = 2f;

    Animator animator;
    Health health;

    public int CurrentSlotNumber { get; set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //timingCollider = GetComponentInChildren<TimingCollider>();
        //fittedBattleCollider = GetComponentInChildren<FittedBattleCollider>();
        health = GetComponent<Health>();
    }

    public void TakeDamage(float damage, float multiplier, bool isTimedAttack)
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

    public IEnumerator AttackedAnimation()
    {
        animator.SetBool("Attacked", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attacked", false);
    }

    public void BlockedAnimation()
    {
        animator.ResetTrigger("Blocked");
        animator.SetTrigger("Blocked");
    }

    public void DeathAnimation()
    {
        animator.SetBool("Dead", true);
    }

    public bool CheckIsDead()
    {
        return health.IsDead();
    }

    public float GetCurrentHP()
    {
        return health.CurrentHP;
    }

}
