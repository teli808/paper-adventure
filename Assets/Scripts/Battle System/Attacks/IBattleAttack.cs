using System.Collections;
public interface IBattleAttack
{
    public IEnumerator AttackSequence(Unit unitTarget);

    public void CalculateDamage();

    public void ConfigureTimingHandler();

    public void OnHitAnimation();
}
