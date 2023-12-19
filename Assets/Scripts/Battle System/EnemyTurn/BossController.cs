using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] Unit enemyToSpawn;
    [SerializeField] string specialAttackKnotName = "specialAttack";
    bool specialEventTriggered = false;

    public override IEnumerator Attack()
    {
        if (GetComponent<Health>().CheckHPBelowPercentage(0.5f) && !specialEventTriggered)
        {
            battleSystem.ChangeBattleState(BattleState.DIALOGUE);

            yield return StartEnemyExtraDialogue(specialAttackKnotName);

            battleSystem.ChangeBattleState(BattleState.CUTSCENE);

            GetComponent<Animator>().SetBool("CastSpell", true);
            yield return new WaitForSeconds(0.5f);
            yield return SpawnEnemyAnimation(1);
            GetComponent<Animator>().SetBool("CastSpell", false);

            yield return new WaitForSeconds(0.5f);

            GetComponent<Animator>().SetBool("CastSpell", true);
            yield return new WaitForSeconds(0.5f);
            yield return SpawnEnemyAnimation(2);
            GetComponent<Animator>().SetBool("CastSpell", false);

            yield return new WaitForSeconds(0.5f);

            battleSystem.ChangeBattleState(BattleState.ENEMYTURN);

            specialEventTriggered = true;

            //yield return base.Attack();
        }
        else
        {
            yield return base.Attack();
        }
    }

    private IEnumerator SpawnEnemyAnimation(int battleSlotNumber)
    {
        Vector3 spawnPosition = BattleSlotManager.Instance.GetEnemyBattleSlot(battleSlotNumber).transform.position;
        Unit tempUnit = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        tempUnit.transform.localScale = Vector3.zero;
        tempUnit.transform.DOScale(1, 1f);
        yield return new WaitForSeconds(1f);

        BattleSlotManager.Instance.InstantiateEnemy(battleSlotNumber, enemyToSpawn);
        Destroy(tempUnit.gameObject);
    }
}


