using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSettings : MonoBehaviour
{
    public BattleInfo BattleInfo { get; set; } = null;
    public string PreviousOverworldScene { get; private set; }
    public string FlagToChangeAfterBattle { get; set; } = null;

    //other info needed:

    //attacks available

    private void Start()
    {
        SetBattleSettingsAfterLoad();
    }

    private void SetBattleSettingsAfterLoad()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        print(sceneName);

        if (sceneName != "BattleScene")
        {
            PreviousOverworldScene = sceneName;

            SetFlagAfterBattle();
        }
    }

    private void SetFlagAfterBattle()
    {
        if (FlagToChangeAfterBattle != null)
        {
            StoryManager.Instance.SetFlagCondition(FlagToChangeAfterBattle);
            FlagToChangeAfterBattle = null;
        }
    }
}
