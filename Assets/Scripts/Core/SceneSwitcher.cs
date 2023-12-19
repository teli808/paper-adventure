using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour, IDataPersistence
{
    Fader fader;

    string sceneNameToLoadInto;

    string battleSceneName = "BattleScene";

    string mainMenuName = "MainMenu";

    private void Awake()
    {
        fader = GameObject.FindWithTag("Fader").GetComponent<Fader>(); //does SceneSwitcher get instantiated first?
    }

    public void TransitionToOverworld(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }
    public void TransitionToBattle()
    {
        DataPersistenceManager.Instance.SaveGame(true);

        StartCoroutine(LoadScene(battleSceneName));
    }

    public void DeathTransition()
    {
        DataPersistenceManager.Instance.ManagePlayerDeath();

        PersistentObjectSpawner.hasSpawned = false; //allow persistentobjects to spawn again when starting game

        LoadSceneAfterDeath();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);

        yield return fader.StartFade(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(fader.StartFade(0f));
    }

    async void LoadSceneAfterDeath() //will continue to run after its object is destroyed
    {
        Destroy(transform.parent.gameObject); //Destroy PersistentObject

        SceneManager.LoadSceneAsync(mainMenuName);
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier(); //SceneSwitcher

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameToLoadInto")))
        {
            sceneNameToLoadInto = ES3.Load<string>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameToLoadInto"));
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier(); //SceneSwitcher

        sceneNameToLoadInto = SceneManager.GetActiveScene().name;

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameToLoadInto"), sceneNameToLoadInto);
    }
}
