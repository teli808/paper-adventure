using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BattleSlotManager : MonoBehaviour
{
    [Header("Needed Prefabs")]
    [SerializeField] Unit playerPrefabDefault;

    [Header("Slots")]
    [SerializeField] BattleSlot playerSlot; //will always be there
    [SerializeField] BattleSlot partnerSlot; //determined by a setting

    [SerializeField] BattleSlot enemySlot1;
    [SerializeField] BattleSlot enemySlot2;
    [SerializeField] BattleSlot enemySlot3;
    [SerializeField] BattleSlot enemySlot4;

    public static BattleSlotManager Instance { get; private set; }

    const int numberOfEnemies = 4;
    BattleSlot[] enemySlots;

    //every method parameters is 1 based

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one BattleSlotManager in the scene");
        }

        Instance = this;

        enemySlots = new BattleSlot[numberOfEnemies] { enemySlot1, enemySlot2, enemySlot3, enemySlot4 };
    }

    private void Start()
    {
        SetUpSlotNumbers();
    }

    public IEnumerator ManageDeadEnemies()
    {
        for (int slotNumber = 1; slotNumber < numberOfEnemies + 1; slotNumber++)
        {
            if (HasUnitInSlot(slotNumber) && CheckIfEnemyDead(slotNumber))
            {
                yield return GetEnemyBattleSlot(slotNumber).DestroyUnit();
            }
        }
    }

    public void InstantiatePlayer(Unit overridePlayerPrefab = null)
    {
        if (overridePlayerPrefab == null) //default behavior
        {
            playerSlot.InstantiateUnit(playerPrefabDefault);
        }
    }

    public void InstantiateEnemy(int slotNumber, Unit enemyUnit)
    {
        if (slotNumber == 0 || slotNumber > numberOfEnemies) Debug.LogError("Incorrect argument to slotNumber");

        GetEnemyBattleSlot(slotNumber).InstantiateUnit(enemyUnit);
    }

    private void SetUpSlotNumbers()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemySlots[i].SlotNumber = i + 1;
        }
    }

    public bool HasUnitInSlot(int slotNumber)
    {
        return GetEnemyBattleSlot(slotNumber).HasUnit();
    }

    public bool CheckEnemyHasOpeningDialogue(int slotNumber) //change
    {
        return GetEnemyUnit(slotNumber).GetComponent<EnemyController>().HasOpeningDialogue;
    }

    public bool CheckEnemyHasEndingDialogue(int slotNumber) //change
    {
        return GetEnemyUnit(slotNumber).GetComponent<EnemyController>().HasEndingDialogue;
    }

    public bool CheckIfEnemyDead(int slotNumber)
    {
        return GetEnemyUnit(slotNumber).CheckIsDead();
    }

    public bool CheckAllEnemiesDead()
    {
        bool allEnemiesDead = true;

        for (int slotNumber = 1; slotNumber < numberOfEnemies + 1; slotNumber++)
        {
            if (HasUnitInSlot(slotNumber))
            {
                allEnemiesDead = false;
                break;
            }
        }

        return allEnemiesDead;
    }

    public Unit GetEnemyUnit(int slotNumber) //check that slot is filled beforehand
    {
        return GetEnemyBattleSlot(slotNumber).CurrentUnit;
    }

    public Unit GetPlayerUnit()
    {
        return playerSlot.CurrentUnit;
    }

    public BattleSlot GetEnemyBattleSlot(int slotNumber)
    {
        return enemySlots[slotNumber - 1];
    }
}
