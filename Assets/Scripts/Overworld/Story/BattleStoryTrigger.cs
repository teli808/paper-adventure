using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStoryTrigger : StoryTrigger
{
    [SerializeField] BattleInfo battleToTrigger;

    BattleSettings battleSettings;

    private void Start()
    {
        battleSettings = GameObject.FindWithTag("BattleSettings").GetComponent<BattleSettings>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!hasBeenTriggered && CheckConditionsFulfilled() && other.tag == "Player")
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        //transition
        EventManager.Instance.ChangePlayerState(PlayerState.DISABLED);
        SetHasBeenTriggered();

        battleSettings.FlagToChangeAfterBattle = tutorialFlagFulfilled.ToString();
        battleSettings.BattleInfo = battleToTrigger; 

        GameObject.FindWithTag("SceneSwitcher").GetComponent<SceneSwitcher>().TransitionToBattle();
    }

}
