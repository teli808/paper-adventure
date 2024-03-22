using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class PlayerChoiceController : MonoBehaviour
{
    [SerializeField] PlayerChoiceBlock attackBlock, itemBlock, specialBlock;
    [SerializeField] float transitionTime = 0.5f;

    PlayerChoiceBlock[] buttonArray;

    bool controllerEnabled = false;

    PlayerChoiceBlock currentSelectedBlock = null;
    PlayerChoiceScreen currentSelectedScreen = null;

    private void Start()
    {
        EnableDefaultController(true);
    }

    private void Update()
    {
        if (!controllerEnabled) return;

        if (currentSelectedBlock != null)
        {
            if (BattleInputManager.Instance.GetMainKeyPressed())
            {
                if (currentSelectedScreen.CheckValidButton())
                {
                    currentSelectedScreen.PressHoveredButton();
                    DisableController();
                }
            }
            else if (BattleInputManager.Instance.GetBlockKeyPressed())
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Battle.BackButton, transform.position);

                currentSelectedScreen.HideScreen();
                currentSelectedBlock = null;
                currentSelectedScreen = null;
            }
            else if (BattleInputManager.Instance.GetNavigateKeyInput() == Vector2.up)
            {
                currentSelectedScreen.Move("up");
            }
            else if (BattleInputManager.Instance.GetNavigateKeyInput() == Vector2.down)
            {
                currentSelectedScreen.Move("down");
            }

            return;
        }

        if (BattleInputManager.Instance.GetNavigateKeyInput() == Vector2.left)
        {
            MoveButtons("left");
        }
        else if (BattleInputManager.Instance.GetNavigateKeyInput() == Vector2.right)
        {
            MoveButtons("right");
        }
        else if (BattleInputManager.Instance.GetMainKeyPressed())
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Battle.AcceptButton, transform.position);

            currentSelectedBlock = SetSelectedButton();

            if (currentSelectedBlock != null)
            {
                currentSelectedScreen = currentSelectedBlock.GetPlayerChoiceScreen();
                currentSelectedScreen.ShowScreenDefault(true);
            }
        }
    }

    public void EnableDefaultController(bool isDefault)
    {
        gameObject.SetActive(true);

        if (isDefault)
        {
            SetDefaultPosition();
        }
        else if (currentSelectedScreen != null)
        {
            currentSelectedScreen.ShowScreenDefault(false);
        }

        controllerEnabled = true;
    }

    public void DisableController()
    {
        controllerEnabled = false;

        if (currentSelectedScreen != null)
        {
            currentSelectedScreen.HideScreen();
        }

        gameObject.SetActive(false);
    }

    private async void MoveButtons(string direction)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Battle.NavigateUI, transform.position);

        controllerEnabled = false;

        SetHoveredButton(false);

        PlayerChoiceBlock[] changedArray = new PlayerChoiceBlock[buttonArray.Length];

        var tasks = new List<Task>();

        if (direction == "left")
        {
            for (int i = 0; i < buttonArray.Length; i++)
            {
                if (i - 1 >= 0)
                {
                    tasks.Add(buttonArray[i].transform.DOMove(buttonArray[i - 1].transform.position, transitionTime).
                        SetEase(Ease.InOutSine).SetLoops(1, LoopType.Yoyo).AsyncWaitForCompletion());
                    buttonArray[i].transform.DOScale(buttonArray[i - 1].transform.localScale, transitionTime);
                    changedArray[i - 1] = buttonArray[i];
                }
                else
                {
                    tasks.Add(buttonArray[0].transform.DOMove(buttonArray[buttonArray.Length - 1].transform.position, transitionTime).
                        SetEase(Ease.InOutSine).SetLoops(1, LoopType.Yoyo).AsyncWaitForCompletion());
                    buttonArray[0].transform.DOScale(buttonArray[buttonArray.Length - 1].transform.localScale, transitionTime);
                    changedArray[buttonArray.Length - 1] = buttonArray[0];
                }
            }
        }
        else if (direction == "right")
        {
            for (int i = 0; i < buttonArray.Length; i++)
            {
                if (i != buttonArray.Length - 1)
                {
                    tasks.Add(buttonArray[i].transform.DOMove(buttonArray[i + 1].transform.position, transitionTime).
                        SetEase(Ease.InOutSine).SetLoops(1, LoopType.Yoyo).AsyncWaitForCompletion());
                    buttonArray[i].transform.DOScale(buttonArray[i + 1].transform.localScale, transitionTime);
                    changedArray[i + 1] = buttonArray[i];
                }
                else
                {
                    tasks.Add(buttonArray[buttonArray.Length - 1].transform.DOMove(buttonArray[0].transform.position, transitionTime).
                        SetEase(Ease.InOutSine).SetLoops(1, LoopType.Yoyo).AsyncWaitForCompletion());
                    buttonArray[buttonArray.Length - 1].transform.DOScale(buttonArray[0].transform.localScale, transitionTime);
                    changedArray[0] = buttonArray[buttonArray.Length - 1];
                }
            }
        }

        buttonArray = changedArray;

        await Task.WhenAll(tasks);

        SetHoveredButton(true);

        controllerEnabled = true;
    }

    private PlayerChoiceBlock SetSelectedButton()
    {
        if (buttonArray[0].GetPlayerChoiceScreen() != null)
        {
            return buttonArray[0];
        }
        else
        {
            print("No screen attached");
        }

        return null;
    }

    private void SetDefaultPosition()
    {
        buttonArray = new PlayerChoiceBlock[] { attackBlock, itemBlock, specialBlock };
        SetHoveredButton(true);

        if (currentSelectedScreen != null)
        {
            currentSelectedScreen.HideScreen();
            currentSelectedScreen = null;
        }

        currentSelectedBlock = null;
    }

    private void SetHoveredButton(bool isHovered)
    {
        buttonArray[0].SetHovered(isHovered);
    }
}
