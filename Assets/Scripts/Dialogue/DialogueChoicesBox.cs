using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueChoicesBox : MonoBehaviour
{
    //Currently does not work with BattleScene

    [SerializeField] const int maxChoices = 4;

    [SerializeField] Button[] buttonArray = new Button[maxChoices];

    Color defaultColor;

    bool isEnabled = false;

    int hoveredChoiceIndex = 0;

    private void Start()
    {
        HideAllComponents();   
    }

    private void Update()
    {
        if (!isEnabled) return;

        if (EventSystem.current.currentSelectedGameObject != buttonArray[hoveredChoiceIndex].gameObject)
        {
            SetActiveButtonIndex();
        }
    }

    public void DisplayChoices(List<Choice> listOfChoices)
    {
        if (listOfChoices.Count > maxChoices)
        {
            Debug.LogError("Error, too many choices for choice box");
        }

        for (int i = 0; i < listOfChoices.Count; i++)
        {
            buttonArray[i].gameObject.SetActive(true);

            TextMeshProUGUI choiceText = buttonArray[i].gameObject.GetComponentInChildren<TextMeshProUGUI>();

            choiceText.text = listOfChoices[i].text;
        }

        EventSystem.current.SetSelectedGameObject(buttonArray[0].gameObject);

        hoveredChoiceIndex = 0;

        isEnabled = true;
    }

    public void DisableChoicesBox()
    {
        isEnabled = false;

        HideAllComponents();
    }

    public int GetChoiceChosen()
    {
        return hoveredChoiceIndex;
    }

    private void SetActiveButtonIndex()
    {
        hoveredChoiceIndex = Array.IndexOf(buttonArray, EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
    }

    private void HideAllComponents()
    {
        int index = 0;

        foreach (Button button in buttonArray)
        {
            button.gameObject.SetActive(false);
            index++;
        }
    }
}
