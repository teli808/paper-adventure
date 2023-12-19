using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyController : MonoBehaviour
{
    protected BattleSystem battleSystem;
    [field: SerializeField] public TextAsset DialogueInkJson { get; private set; }

    [field: SerializeField] public bool HasOpeningDialogue { get; private set; } = false;
    [field: SerializeField] public bool HasEndingDialogue { get; private set; } = false;

    private void Awake()
    {
        if (HasOpeningDialogue || HasEndingDialogue) Assert.IsNotNull(DialogueInkJson);

        //ensure the appropriate knots are in the ink files

        battleSystem = GameObject.FindWithTag("BattleSystem").GetComponent<BattleSystem>();
    }

    public virtual IEnumerator Attack()
    {
        Unit playerUnit = BattleSlotManager.Instance.GetPlayerUnit();

        IBattleAttack chosenAttack = GetComponent<IBattleAttack>();

        yield return chosenAttack.AttackSequence(playerUnit);
    }

    protected IEnumerator StartEnemyExtraDialogue(string knotName)
    {
        yield return battleSystem.StartEnemyExtraDialogue(GetComponent<Unit>().CurrentSlotNumber, knotName);
        
    }
}
