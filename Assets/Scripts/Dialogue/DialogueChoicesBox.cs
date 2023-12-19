using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;

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
        GetDefaultColor();
        HideAllComponents();   
    }

    private void Update()
    {
        //print(EventSystem.current.currentSelectedGameObject);

        if (!isEnabled) return;

        if (EventSystem.current.currentSelectedGameObject != buttonArray[hoveredChoiceIndex].gameObject)
        {
            ChangeHoveredButtonTextColor(false, hoveredChoiceIndex);
            SetActiveButtonIndex();
            ChangeHoveredButtonTextColor(true, hoveredChoiceIndex);
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

        ChangeHoveredButtonTextColor(true, hoveredChoiceIndex);

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

    private void GetDefaultColor()
    {
        defaultColor = buttonArray[0].GetComponentInChildren<TextMeshProUGUI>().color;
    }
    private void SetActiveButtonIndex()
    {
        hoveredChoiceIndex = Array.IndexOf(buttonArray, EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
    }

    private void ChangeHoveredButtonTextColor(bool isHovered, int buttonNumber)
    {
        TextMeshProUGUI buttonTMP = buttonArray[buttonNumber].GetComponentInChildren<TextMeshProUGUI>();

        if (isHovered)
        {
            buttonTMP.color = Color.red;
        }
        else
        {
            buttonTMP.color = defaultColor;
        }

        
    }

    private void HideAllComponents()
    {
        int index = 0;

        foreach (Button button in buttonArray)
        {
            ChangeHoveredButtonTextColor(false, index);
            button.gameObject.SetActive(false);
            index++;
        }
    }
}
