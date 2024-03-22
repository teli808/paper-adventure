using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MeleeAttack, IBattleAttack
{
    [SerializeField] ParticleSystem jumpParticleSystem;

    [Header("Jump settings")]
    [SerializeField] float runningSpeed = 1f;
    [SerializeField] float runningStopDistance = 0.5f;
    [SerializeField] float timeToJump = 1f;
    [SerializeField] float damageMultiplier = 2;

    TimingHandler timingHandler;
    Animator animator;

    Unit attackingUnit;
    Unit unitTarget;

    Vector3 startingPosition;

    private void Awake()
    {
        attackingUnit = GetComponent<Unit>();
        animator = GetComponentInChildren<Animator>();
        timingHandler = GetComponent<TimingHandler>();
    }

    public IEnumerator AttackSequence(Unit unitTarget)
    {
        this.unitTarget = unitTarget;
        startingPosition = transform.position;

        ConfigureTimingHandler();

        CameraManager.Instance.SetAttackZoomOnCamera(unitTarget);
        yield return RunTowards(unitTarget.transform.position, runningStopDistance);
        yield return JumpTowards();
        CameraManager.Instance.DefaultViewCamera();
        yield return RunTowards(startingPosition, 0f);
    }

    private IEnumerator RunTowards(Vector3 endingPosition, float stopDistance)
    {
        animator.SetBool("Running", true);

        while (Vector3.Distance(transform.position, endingPosition) > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, endingPosition, runningSpeed * Time.deltaTime);

            yield return null;
        }

        animator.SetBool("Running", false);
    }

    private IEnumerator JumpTowards()
    {
        animator.SetBool("Jumping", true);

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Battle.PlayerJump, attackingUnit.transform.position);

        Vector3 startingPositionOfJump = transform.position;

        Vector3 center = (transform.position + unitTarget.transform.position) * 0.5F;
        center -= new Vector3(0, 0.01f, 0);
        Vector3 beginningPos = transform.position - center;
        Vector3 positionofEnemy = unitTarget.transform.position - center;

        float elapsedTime = 0f;

        timingHandler.EnableHandling();
        bool hasHitTarget = false;

        while (elapsedTime < timeToJump && !hasHitTarget)
        {
            transform.position = Vector3.Slerp(beginningPos, positionofEnemy, elapsedTime / timeToJump);
            transform.position += center;
            elapsedTime += Time.deltaTime;

            if (CheckIfHitTarget())
            {
                hasHitTarget = true;
            }

            yield return null;
        }

        timingHandler.DisableHandling();
        CalculateDamage();
        OnHitAnimation();

        center = (transform.position + startingPositionOfJump) * 0.5F;
        center -= new Vector3(0, 0.01f, 0);
        Vector3 currentPos = transform.position - center;
        beginningPos = startingPositionOfJump - center;

        elapsedTime = 0f;

        while (elapsedTime < timeToJump)
        {
            transform.position = Vector3.Slerp(currentPos, beginningPos, elapsedTime / timeToJump);
            transform.position += center;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("Jumping", false);

    }

    private bool CheckIfHitTarget()
    {
        return unitTarget.FittedBattleCollider.CurrentlyTouching;
    }


    public void CalculateDamage()
    {
        unitTarget.TakeDamage(attackingUnit.GetAttackDamage(), damageMultiplier, timingHandler.IsTimedCorrectly);
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
        PlayParticles();
    }

    private void PlayParticles()
    {
        var shape = jumpParticleSystem.shape;
        shape.radius = unitTarget.FittedBattleCollider.GetCapsuleRadius();

        jumpParticleSystem.transform.position = unitTarget.FittedBattleCollider.GetCapsuleBoundsCenter();

        Instantiate(jumpParticleSystem);

    }

    public void ConfigureTimingHandler()
    {
        timingHandler.SetUpTimingHandler(unitTarget, attackingUnit.FittedBattleCollider.GetUnitCollider());
        timingHandler.ResetInputSettings();
    }
}
