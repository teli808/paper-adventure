using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMOverworldEnum
{
    TUTORIALBGM,
}

public enum BGMBattleEnum
{
    DEFAULTBATTLEBGM,
    WIZARDBOSSBGM
}

public class BGMHandler : MonoBehaviour
{
    string battleSceneName = "BattleScene";

    AudioManager audioManager;

    EventInstance currentlyPlayingBGM;

    // Overworld instances
    EventInstance tutorialBGM;

    // Battle instances
    EventInstance defaultBattleBGM;
    EventInstance wizardBossBGM;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        tutorialBGM = audioManager.Create2DEventInstance(FMODEvents.Instance.Overworld.TutorialBGM);
        defaultBattleBGM = audioManager.Create2DEventInstance(FMODEvents.Instance.Battle.DefaultBattleBGM);
        wizardBossBGM = audioManager.Create2DEventInstance(FMODEvents.Instance.Battle.WizardBossBGM);

        if (SceneManager.GetActiveScene().name != battleSceneName)
        {
            StartOverworldBGM(BGMOverworldEnum.TUTORIALBGM);//add conditional in case game is started in battle mode first (testing)
        }
    }

    public void StartBattleBGM(BGMBattleEnum bgmBattleEnum)
    {
        audioManager.StopEventInstance(currentlyPlayingBGM, STOP_MODE.IMMEDIATE);

        switch (bgmBattleEnum)
        {
            case BGMBattleEnum.DEFAULTBATTLEBGM:
                defaultBattleBGM.start();
                currentlyPlayingBGM = defaultBattleBGM;
                break;
            case BGMBattleEnum.WIZARDBOSSBGM:
                wizardBossBGM.start();
                currentlyPlayingBGM = wizardBossBGM;
                break;
        }
    }

    public void StartOverworldBGM(BGMOverworldEnum bgmOverworldEnum)
    {
        audioManager.StopEventInstance(currentlyPlayingBGM, STOP_MODE.IMMEDIATE);

        switch (bgmOverworldEnum)
        {
            case BGMOverworldEnum.TUTORIALBGM:
                tutorialBGM.start();
                currentlyPlayingBGM = tutorialBGM;
                break;
        }
    }
}
