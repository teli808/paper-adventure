using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseEnemyHandler : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] PlayerChoiceController playerChoiceController;
    [SerializeField] UnityEngine.GameObject cursor;
    [SerializeField] float waitBetweenPresses = 0.3f;

    Camera cam;

    AttackChoices currentAttackChoice;
    bool handlerEnabled = false;
    int numberOfMaxEnemies = 4;
    int currentEnemySlot = 1;
    float timeSinceLastPress = Mathf.Infinity;

    private void Awake()
    {
        cam = Camera.main;
        DisableEnemyHandler();
    }

    private void Update()
    {
        if (!handlerEnabled) return;

        timeSinceLastPress += Time.deltaTime;

        if (BattleInputManager.Instance.GetMainKeyPressed())
        {
            TargetChosen();
        }
        else if (BattleInputManager.Instance.GetBlockKeyPressed())
        {
            DisableEnemyHandler();
            playerChoiceController.EnableDefaultController(false);
        }
        else if (BattleInputManager.Instance.GetNavigateKeyInput() == Vector2.left)
        {
            if (timeSinceLastPress < waitBetweenPresses) return;

            MoveCursorLeft();

            while (CheckNoEnemyExists())
            {
                MoveCursorLeft();
            }

            SetCursor();

            timeSinceLastPress = 0;
        }
        else if (BattleInputManager.Instance.GetNavigateKeyInput() == Vector2.right)
        {
            if (timeSinceLastPress < waitBetweenPresses) return;

            MoveCursorRight();

            while (CheckNoEnemyExists())
            {
                MoveCursorRight();
            }

            SetCursor();

            timeSinceLastPress = 0;
        }
    }

    private void TargetChosen()
    {
        DisableEnemyHandler();
        StartCoroutine(battleSystem.StartPlayerAttack(currentAttackChoice, currentEnemySlot));
    }

    public void EnableEnemyHandler(AttackChoices attackChoice)
    {
        currentEnemySlot = 1;
        currentAttackChoice = attackChoice;

        while (CheckNoEnemyExists())
        {
            MoveCursorRight();
        }

        SetCursor();

        cursor.GetComponent<Image>().enabled = true;

        handlerEnabled = true;
    }

    private void DisableEnemyHandler()
    {
        handlerEnabled = false;
        cursor.GetComponent<Image>().enabled = false;
    }

    private void SetCursor()
    {
        UnityEngine.GameObject unit = BattleSlotManager.Instance.GetEnemyUnit(currentEnemySlot).gameObject;
        Renderer unitRenderer = unit.GetComponentInChildren<Renderer>();

        Vector3 aboveFirstUnit = unit.transform.position + new Vector3(0, unitRenderer.bounds.size.y / 2 + 0.15f, 0);
        Vector3 screenPos = cam.WorldToScreenPoint(aboveFirstUnit);
        cursor.transform.position = screenPos;
    }

    private void MoveCursorRight()
    {
        if (currentEnemySlot + 1 > numberOfMaxEnemies)
        {
            currentEnemySlot = 1;
        }
        else
        {
            currentEnemySlot += 1;
        }
    }

    private void MoveCursorLeft()
    {
        if (currentEnemySlot - 1 == 0)
        {
            currentEnemySlot = numberOfMaxEnemies;
        }
        else
        {
            currentEnemySlot -= 1;
        }
    }

    private bool CheckNoEnemyExists()
    {
        return !BattleSlotManager.Instance.HasUnitInSlot(currentEnemySlot);
    }
}
