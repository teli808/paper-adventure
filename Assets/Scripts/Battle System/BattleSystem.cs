using System;
using System.Collections;
using UnityEngine;

public enum BattleState { SETUP, DIALOGUE, CUTSCENE, PLAYERUI, PLAYERATTACK, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState battleState;

    [SerializeField] BattleSlotManager battleSlotManager;
    [SerializeField] PlayerChoiceController playerChoiceController;
    [SerializeField] BattleInfo battleInfo;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] float timeScaleMultiplier = 1f; //for testing

    BattleSettings battleSettings;
    SceneSwitcher sceneSwitcher;

    const int numberOfEnemies = 4;

    bool isDialogueCompleted = false;

    string dialogueOpeningKnotName = "openingDialogue";
    string dialogueEndingKnotName = "endingDialogue";

    //public event Action battleWon; //do onEnable next
    //public event Action battleLost;

    private void OnEnable()
    {
        EventManager.Instance.OnDialogueOver += SetIsDialogueCompleted;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDialogueOver -= SetIsDialogueCompleted;
    }

    void Start()
    {
        battleSettings = GameObject.FindWithTag("BattleSettings").GetComponent<BattleSettings>();
        sceneSwitcher = GameObject.FindWithTag("SceneSwitcher").GetComponent<SceneSwitcher>();

        ChangeBattleState(BattleState.SETUP);
        StartCoroutine(SetUpBattle());
    }

    private void Update()
    {
        if (Time.timeScale != timeScaleMultiplier)
        {
            Time.timeScale = timeScaleMultiplier;
        }

        if (battleState == BattleState.DIALOGUE && BattleInputManager.Instance.GetMainKeyPressed())
        {
            EventManager.Instance.DialogueSubmitted();
        }

    }

    private IEnumerator SetUpBattle()
    {
        //for testing
        if (battleSettings.GetComponent<BattleSettings>().BattleInfo != null)
        {
            battleInfo = battleSettings.GetComponent<BattleSettings>().BattleInfo;
        }
        //end for testing

        CameraManager.Instance.DefaultViewCamera();

        InstantiatePlayer();
        InstantiateEnemies();

        yield return new WaitForSeconds(1.0f);

        for (int i = 1; i < numberOfEnemies + 1; i++)
        {
            if (!battleSlotManager.HasUnitInSlot(i)) continue;

            if (battleSlotManager.CheckEnemyHasOpeningDialogue(i))
            {
                yield return StartInitialDialogue(i);
                yield return new WaitUntil(() => isDialogueCompleted);
                isDialogueCompleted = false;
            }
        }

        EventManager.Instance.BattleSetUp();

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        ChangeBattleState(BattleState.PLAYERUI);

        CameraManager.Instance.DefaultViewCamera();
        playerChoiceController.EnableDefaultController(true);
    }

    public IEnumerator StartPlayerAttack(AttackChoices attackChoice, int enemyNumber)
    {
        ChangeBattleState(BattleState.PLAYERATTACK);

        yield return battleSlotManager.GetPlayerUnit().GetComponent<AttackController>().Attack(attackChoice, enemyNumber);

        yield return ManageDeadEnemies();

        if (CheckBattleDone())
        {
            StartCoroutine(StartBattleDone());
        }
        else
        {
            StartCoroutine(EnemyAttacks());
        }
    }

    private IEnumerator EnemyAttacks()
    {
        ChangeBattleState(BattleState.ENEMYTURN);

        yield return new WaitForSeconds(1.5f);

        for (int slotNumber = 1; slotNumber < numberOfEnemies + 1; slotNumber++)
        {
            if (CheckBattleDone())
            {
                StartCoroutine(StartBattleDone());
                yield break;
            }

            if (!battleSlotManager.HasUnitInSlot(slotNumber)) continue;

            yield return battleSlotManager.GetEnemyUnit(slotNumber).GetComponent<EnemyController>().Attack();
        }

        if (CheckBattleDone())
        {
            StartCoroutine(StartBattleDone());
        }
        else
        {
            StartPlayerTurn();
        }
    }

    private IEnumerator StartBattleDone()
    {
        CameraManager.Instance.SetTargetPlayerAttackCamera();

        Time.timeScale = 1f; // set timescale to 1 in case it was changed during testing

        if (battleState == BattleState.LOST)
        {
            //call event onBattleLost() created it at the top if needed for sound system, etc
            yield return DeathTransition();

        }
        else if (battleState == BattleState.WON)
        {
            //call event onBattleWon() created it at the top if needed for sound system, etc
            //battleSlotManager.GetPlayerUnit().DeathAnimation();
            print("BATTLE WON, PRESS F"); //eventually have a cutscene play, don't need input
            yield return WaitUserInputBattleEnd();
            print("Transitioning to overworld");
            OverworldTransition();
        }
        yield return null;
    }

    private bool CheckBattleDone()
    {
        if (IsPlayerDead())
        {
            battleState = BattleState.LOST; //change
            return true;
        }

        if (CheckAllEnemiesDead())
        {
            battleState = BattleState.WON; //change
            return true;
        }

        return false;

    }

    private IEnumerator ManageDeadEnemies()
    {
        for (int battleSlot = 1; battleSlot < numberOfEnemies + 1; battleSlot++)
        {
            if (!battleSlotManager.HasUnitInSlot(battleSlot)) continue;

            if (battleSlotManager.CheckEnemyHasEndingDialogue(battleSlot) && battleSlotManager.CheckIfEnemyDead(battleSlot))
            {
                yield return StartEndingDialogue(battleSlot);
                yield return new WaitUntil(() => isDialogueCompleted);
                isDialogueCompleted = false;
            }
        }

        yield return battleSlotManager.ManageDeadEnemies();
    }

    private IEnumerator StartInitialDialogue(int slotNumber)
    {
        ChangeBattleState(BattleState.DIALOGUE);

        TextAsset dialogueFile = battleSlotManager.GetEnemyUnit(slotNumber).GetComponent<EnemyController>().DialogueInkJson;

        CameraManager.Instance.SetZoomOnEnemyDialogueCamera();

        yield return new WaitForSeconds(0.5f);

        DialogueManager.Instance.EnterDialogueMode(dialogueFile, dialogueOpeningKnotName);
    }

    private IEnumerator StartEndingDialogue(int slotNumber)
    {
        ChangeBattleState(BattleState.DIALOGUE);

        TextAsset dialogueFile = battleSlotManager.GetEnemyUnit(slotNumber).GetComponent<EnemyController>().DialogueInkJson;

        CameraManager.Instance.SetZoomOnEnemyDialogueCamera();

        yield return new WaitForSeconds(0.5f);

        DialogueManager.Instance.EnterDialogueMode(dialogueFile, dialogueEndingKnotName);
    }

    public IEnumerator StartEnemyExtraDialogue(int slotNumber, string knotName)
    {
        //change battle state from controller 

        TextAsset dialogueFile = battleSlotManager.GetEnemyUnit(slotNumber).GetComponent<EnemyController>().DialogueInkJson;

        CameraManager.Instance.SetZoomOnEnemyDialogueCamera();

        yield return new WaitForSeconds(0.5f);

        DialogueManager.Instance.EnterDialogueMode(dialogueFile, knotName);

        yield return new WaitUntil(() => isDialogueCompleted);
        isDialogueCompleted = false;
    }

    public void ChangeBattleState(BattleState newBattleState) //refactor to use this in BattleSystem?
    {
        battleState = newBattleState;
        BroadcastNewBattleState(battleState);
    }

    private bool CheckAllEnemiesDead()
    {
        return battleSlotManager.CheckAllEnemiesDead();
    }

    private IEnumerator WaitUserInputBattleEnd()
    {
        while (!BattleInputManager.Instance.GetMainKeyPressed())
        {
            yield return null;
        }
    }

    private void OverworldTransition()
    {
        sceneSwitcher.TransitionToOverworld(battleSettings.PreviousOverworldScene);
    }

    private IEnumerator DeathTransition()
    {
        battleSlotManager.GetPlayerUnit().DeathAnimation();

        gameOverCanvas.enabled = true;
        yield return new WaitForSeconds(2f);
        sceneSwitcher.DeathTransition();
    }

    private void SetIsDialogueCompleted()
    {
        isDialogueCompleted = true;
    }

    private bool IsPlayerDead()
    {
        return battleSlotManager.GetPlayerUnit().CheckIsDead();
    }

    private void InstantiatePlayer()
    {
        if (battleInfo.PlayerOverride == null) //default player
        {
            battleSlotManager.InstantiatePlayer();
        }
    }

    private void InstantiateEnemies()
    {
        Unit[] battleInfoEnemies = battleInfo.EnemyUnits;

        for (int i = 0; i < battleInfoEnemies.Length; i++)
        {
            if (battleInfoEnemies[i] == null) continue;

            battleSlotManager.InstantiateEnemy(i + 1, battleInfoEnemies[i]);
        }
    }

    private void BroadcastNewBattleState(BattleState battleState)
    {
        EventManager.Instance.ChangedBattleState(battleState);
    }
}
