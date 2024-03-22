using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : ProjectileAttack, IBattleAttack
{
    [SerializeField] FireballProjectile attackingProjectilePrefab;

    [SerializeField] Transform projectileInstantiatePoint;

    [Header("Projectile settings")]
    [SerializeField] float projectileVelocity = 2f;
    [SerializeField] float damageMultiplier = 2;

    [SerializeField] float fadeOutTime = 0.5f; //put into particle system?

    TimingHandler timingHandler;
    Animator animator;

    Unit unitTarget;
    Unit attackingUnit;

    FireballProjectile instantiatedProjectile;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        timingHandler = GetComponent<TimingHandler>();
        attackingUnit = GetComponent<Unit>();
    }

    public IEnumerator AttackSequence(Unit unitTarget)
    {
        this.unitTarget = unitTarget;

        yield return InitialAnimation();

        instantiatedProjectile = Instantiate(attackingProjectilePrefab, projectileInstantiatePoint);

        ConfigureTimingHandler();

        yield return MoveProjectile();

        yield return new WaitForSeconds(1f);

        animator.SetBool("CastSpell", false);
    }

    private IEnumerator InitialAnimation()
    {
        animator.SetBool("CastSpell", true);

        yield return new WaitForSeconds(0.75f);
    }

    private IEnumerator MoveProjectile()
    {
        timingHandler.EnableHandling();

        while (unitTarget.FittedBattleCollider.GetCapsuleBoundsCenter().x < instantiatedProjectile.GetComponent<Collider>().bounds.min.x)
        {
            instantiatedProjectile.transform.position = Vector3.MoveTowards(instantiatedProjectile.transform.position, unitTarget.transform.position, projectileVelocity * Time.deltaTime);
            yield return null;
        }

        timingHandler.DisableHandling();

        CalculateDamage();
        OnHitAnimation();

        StartCoroutine(EndingProjectileBehavior());
    }

    private IEnumerator EndingProjectileBehavior()
    {
        ParticleSystem fireballParticleSystem = instantiatedProjectile.FireballParticles;

        fireballParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        instantiatedProjectile.ExplosionParticles.Play();

        instantiatedProjectile.SetRemainingParticlesLifetime(fireballParticleSystem, fadeOutTime);

        StartCoroutine(instantiatedProjectile.DestroyProjectile());

        yield return null;
    }

    public void CalculateDamage()
    {
        unitTarget.TakeDamage(attackingUnit.GetAttackDamage(), damageMultiplier, timingHandler.IsTimedCorrectly);
    }

    public void ConfigureTimingHandler()
    {
        timingHandler.SetUpTimingHandler(unitTarget, instantiatedProjectile.GetComponent<Collider>());
        timingHandler.ResetInputSettings();
    }

    public void OnHitAnimation()
    {
        if (timingHandler.IsTimedCorrectly)
        {
            StartCoroutine(unitTarget.UnitAnimator.TimedCorrectlyAnimation());
        }
        else
        {
            StartCoroutine(unitTarget.UnitAnimator.AttackedAnimation());
        }
    }
}
