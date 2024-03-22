using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    JumpAttack jumpAttack;

    private void Awake()
    {
        jumpAttack = GetComponent<JumpAttack>();
    }

    public IEnumerator Attack(AttackChoices attackChoice, int enemyNumber)
    {
        Unit enemyUnit = BattleSlotManager.Instance.GetEnemyUnit(enemyNumber);

        switch (attackChoice)
        {
            case AttackChoices.JUMP:
                yield return jumpAttack.AttackSequence(enemyUnit);
                break;
            default:
                break;
        }
    }
}
