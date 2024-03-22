using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialogue : MonoBehaviour, IInteractable, IDataPersistence
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; 

    [SerializeField] TutorialDialogueBranch[] tutorialDialogueBranches;

    [System.Serializable]
    public class TutorialDialogueBranch
    {
        public TutorialFlagsEnum tutorialFlagNeeded;
        public TutorialFlagsEnum tutorialFlagFulfilled;
        public bool hasBeenTriggered;
        public string inkKnot;
        public ItemEventTrigger triggerItemEvent;
    }

    private bool isPlaying = false;
    private bool isDialogueCompleted = false;

    private bool playerInRange = false;
    private bool receivedUserInput = false;

    GameObject IInteractable.gameObject { get => gameObject;}

    private void Awake()
    {
        visualCue.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnDialogueOver += OnDialogueOver;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDialogueOver -= OnDialogueOver;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.Instance.DialogueIsPlaying)
        {
            visualCue.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
        }

        if (receivedUserInput)
        {
            receivedUserInput = false;
            StartCoroutine(DetermineDialogue());
        }
    }

    private IEnumerator DetermineDialogue()
    {
        isDialogueCompleted = false;
        isPlaying = true;

        for (int i = 0; i < tutorialDialogueBranches.Length; i++)
        {
            TutorialDialogueBranch branch = tutorialDialogueBranches[i];

            if (!branch.hasBeenTriggered && CheckConditionFulfilled(branch.tutorialFlagNeeded))
            {
                DialogueManager.Instance.EnterDialogueMode(inkJSON, branch.inkKnot);

                yield return new WaitUntil(() => isDialogueCompleted);

                if (DialogueManager.Instance.StoryContainsDialogueChoiceVar && !DialogueManager.Instance.SearchDialogueChoiceVar())
                { //If there is a dialogue choice var in ink file, but it evaluates to false after story is complete
                    yield break;
                }
                else
                {
                    branch.hasBeenTriggered = true;
                    SetFlag(branch.tutorialFlagFulfilled);
                    CheckItemEventTrigger(branch.triggerItemEvent);

                    yield break;
                }
            }
        }

        DialogueManager.Instance.EnterDialogueMode(inkJSON);
    }

    private void CheckItemEventTrigger(ItemEventTrigger itemObtainedEventTrigger)
    {
        if (itemObtainedEventTrigger != null)
        {
            itemObtainedEventTrigger.StartItemEvent();
        }
    }

    public void OnDialogueOver()
    {
        if (isPlaying)
        {
            isDialogueCompleted = true;

            EventManager.Instance.ChangePlayerState(PlayerState.IDLING);

            isPlaying = false;
        }
    }

    public void HandleUserInteraction()
    {
        receivedUserInput = true;
    }

    private bool CheckConditionFulfilled(TutorialFlagsEnum flag)
    {
        return StoryManager.Instance.CheckFlagCondition(flag.ToString());
    }

    private void SetFlag(TutorialFlagsEnum flag)
    {
        StoryManager.Instance.SetFlagCondition(flag.ToString());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "tutorialDialogueBranches")))
        {
            tutorialDialogueBranches = ES3.Load<TutorialDialogueBranch[]>
                (SaveKeyCreator.CreateFullKey(uniqueIdentifier, "tutorialDialogueBranches"));
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "tutorialDialogueBranches"), tutorialDialogueBranches);
    }
}
