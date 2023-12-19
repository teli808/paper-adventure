using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StoryDialogueTrigger : StoryTrigger
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [SerializeField] CutsceneTrigger shouldLeadIntoCutscene;

    [SerializeField] private BattleInfo shouldLeadIntoBattle;

    private void Start()
    {
        //Assert.IsFalse(shouldLeadIntoCutscene && shouldLeadIntoBattle == true);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnDialogueOver += OnDialogueOver;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDialogueOver -= OnDialogueOver;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && CheckConditionsFulfilled() && other.tag == "Player")
        {
            StartDialogue();
        }
    }


    public void StartDialogue()
    {
        EventManager.Instance.ChangePlayerState(PlayerState.TALKING);
        DialogueManager.Instance.EnterDialogueMode(inkJSON);
        isPlaying = true;
    }

    public void OnDialogueOver()
    {
        if (isPlaying)
        {
            SetHasBeenTriggered();
            SetFlag();
            isPlaying = false;

            if (shouldLeadIntoBattle != null)
            {
                //async delay?
            }
            else if (shouldLeadIntoCutscene != null)
            {
                //async delay?
                shouldLeadIntoCutscene.StartCutscene();
            }

            //Player control is given back via DialogueManager
        }
    }
}
