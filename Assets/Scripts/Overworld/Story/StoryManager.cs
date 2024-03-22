using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    [SerializeField] Flags storyFlags;

    [SerializeField] TutorialFlagsEnum beginningAutoSaveEnum = TutorialFlagsEnum.FIRST_CUTSCENE_PLAYED;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Story Manager in the scene");
        }
        Instance = this;

        print("StoryManager setup");
    }

    public bool CheckFlagCondition(string givenEnum)
    {
        if (givenEnum == "NONE") return true;

        return storyFlags.CheckCondition(givenEnum);
    }

    public void SetFlagCondition(string givenEnum)
    {
        if (givenEnum == "NONE") return;

        storyFlags.SetCondition(givenEnum);

        CheckAutoSaveAtStart(givenEnum);

        print("Current condition: " + givenEnum);
    }

    private void CheckAutoSaveAtStart(string givenEnum)
    {
        if (givenEnum == beginningAutoSaveEnum.ToString())
        {
            DataPersistenceManager.Instance.SaveGame();
            DataPersistenceManager.Instance.CreatePlayerSaveFile();
            print("Auto saved at " + givenEnum);
        }
    }
}
