using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class CutsceneTrigger : StoryTrigger
{
    [SerializeField] PlayableDirector playableDirector;

    [SerializeField] StoryDialogueTrigger shouldLeadIntoDialogue;

    [SerializeField] private BattleInfo shouldLeadIntoBattle;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        //Assert.IsFalse(shouldLeadIntoDialogue && shouldLeadIntoBattle == true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && CheckConditionsFulfilled() && other.tag == "Player")
        {
            StartCutscene();
        }
    }

    public void StartCutscene()
    {
        EventManager.Instance.ChangePlayerState(PlayerState.DISABLED);
        playableDirector.Play();
        isPlaying = true;
        print("Cutscene Triggered");
    }

    public async void OnCutSceneOver() //sub to relevant event
    {
        if (isPlaying)
        {
            //change below part to event when dialogue is finished and check if a battle is needed
            SetHasBeenTriggered();
            SetFlag();
            isPlaying = false;
            print("CutsceneOver");

            if (shouldLeadIntoBattle != null)
            {
                //
            }
            else if (shouldLeadIntoDialogue != null)
            {
                //
            }
            else
            {
                await Task.Delay(250);
                EventManager.Instance.ChangePlayerState(PlayerState.IDLING);
            }
        }
    }
}
