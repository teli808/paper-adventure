using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class CutsceneTrigger : StoryTrigger //Music should be played via a signal to AudioManager, not in cutscene itself
{
    [SerializeField] PlayableDirector playableDirector;

    [SerializeField] TextAsset inkJSON; // can be null

    [SerializeField] private BattleInfo shouldLeadIntoBattle;

    bool shouldLoopPlayableDirector = false;

    double timeToLoopFrom = 0f;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
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
            else
            {
                await Task.Delay(250);
                EventManager.Instance.ChangePlayerState(PlayerState.IDLING);
            }
        }
    }

    public void StartCutsceneDialogue(string optionalKnot = null)
    {
        timeToLoopFrom = playableDirector.time;
        shouldLoopPlayableDirector = true;

        EventManager.Instance.ChangePlayerState(PlayerState.TALKING_IN_CUTSCENE);

        DialogueManager.Instance.EnterDialogueMode(inkJSON, optionalKnot);

        //playableDirector.Pause();

        //playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void OnDialogueOver()
    {
        EventManager.Instance.ChangePlayerState(PlayerState.DISABLED);

        shouldLoopPlayableDirector = false;

        //playableDirector.Resume();

        //playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public void LoopDuringDialogue() // Called from LoopBackOnDialogueSignal -> playableDirector.time loops back to timeToLoopFrom
    {
        if (shouldLoopPlayableDirector)
        {
            playableDirector.time = timeToLoopFrom;
        }
    }
}
