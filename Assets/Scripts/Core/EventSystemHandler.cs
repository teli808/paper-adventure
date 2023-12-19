using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] InputSystemUIInputModule overworldUIInputModule;
    [SerializeField] InputSystemUIInputModule battleUIInputModule;

    [SerializeField] string battleSceneName = "BattleScene";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ChangeModuleOnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ChangeModuleOnSceneLoad;
    }

    private void ChangeModuleOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == battleSceneName)
        {
            overworldUIInputModule.enabled = false;
            battleUIInputModule.enabled = true;
        }
        else
        {
            battleUIInputModule.enabled = false;
            overworldUIInputModule.enabled = true;
        }
    }
}
