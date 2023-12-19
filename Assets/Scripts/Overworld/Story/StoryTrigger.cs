using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveId))]
public class StoryTrigger : MonoBehaviour, IDataPersistence
{
    [Header("Story Conditions")]
    [SerializeField] protected TutorialFlagsEnum[] tutorialFlagsNeeded; //include an array for each set of level conditions later
    [SerializeField] protected TutorialFlagsEnum tutorialFlagFulfilled;

    [SerializeField] protected bool hasBeenTriggered = false;
    protected bool isPlaying = false;

    //[SerializeField] protected BattleInfo shouldLeadIntoBattle; //add logic

    protected bool CheckConditionsFulfilled()
    {
        foreach (TutorialFlagsEnum flag in tutorialFlagsNeeded)
        {
            if (!StoryManager.Instance.CheckFlagCondition(flag.ToString()))
            {
                return false;
            }
        }

        return true;
    }

    protected void SetHasBeenTriggered()
    {
        hasBeenTriggered = true;
    }
    protected void SetFlag()
    {
        StoryManager.Instance.SetFlagCondition(tutorialFlagFulfilled.ToString());
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "hasBeenTriggered")))
        {
            hasBeenTriggered = ES3.Load<bool>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "hasBeenTriggered"));
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "hasBeenTriggered"), hasBeenTriggered);
    }
}
