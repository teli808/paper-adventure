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
        DisableAll();

        if (scene.name == battleSceneName)
        {
            battleUIInputModule.enabled = true;
        }
        else
        {
            overworldUIInputModule.enabled = true;
        }
    }

    private void DisableAll()
    {
        battleUIInputModule.enabled = false;
        overworldUIInputModule.enabled = false;
    }
}
