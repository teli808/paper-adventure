using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InkVariableObserver : MonoBehaviour
{
    //Current function: If there was a choice that affects story enum, 
    //search for a universal global variable in ink json story
    //after completion and maintain this until the next dialogue.
    //Choices will always ultimately lead to a simple boolean yes/no.

    //Used by: InteractableDialogueTrigger

    [SerializeField] string dialogueChoiceVarName = "correctChoiceChosen";

    public bool ContainsDialogueChoiceVar(Story finishedStory)
    {
        return finishedStory.variablesState.Contains(dialogueChoiceVarName);
    }

    public bool SearchDialogueChoiceVar(Story finishedStory)
    {
        return (bool) finishedStory.variablesState[dialogueChoiceVarName];
    }
}
