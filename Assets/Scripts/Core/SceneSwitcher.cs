using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour, IDataPersistence //Tag: SceneSwitcher
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
        AudioManager.Instance.BGMHandler.StartOverworldBGM(BGMOverworldEnum.TUTORIALBGM);

        DataPersistenceManager.Instance.SetShouldLoadData(true);

        StartCoroutine(LoadScene(sceneName));
    }

    public IEnumerator TransitionOverworldToOverworld(string sceneName) //Do not need to restart BGM
    {
        DataPersistenceManager.Instance.SetShouldLoadData(true); // The problem is triggers reset

        yield return LoadScene(sceneName);
    }

    public void TransitionToNormalBattle()
    {
        DataPersistenceManager.Instance.SaveGame();
        DataPersistenceManager.Instance.SetShouldLoadData(false);

        AudioManager.Instance.BGMHandler.StartBattleBGM(BGMBattleEnum.DEFAULTBATTLEBGM);

        StartCoroutine(LoadScene(battleSceneName));
    }

    public IEnumerator TransitionToBossBattle() //provide option to change boss music
    {
        DataPersistenceManager.Instance.SaveGame();
        DataPersistenceManager.Instance.SetShouldLoadData(false);

        AudioManager.Instance.BGMHandler.StartBattleBGM(BGMBattleEnum.DEFAULTBATTLEBGM);

        yield return new WaitForSeconds(1f); //Wait longer for special emphasis

        StartCoroutine(LoadSceneLongFade(battleSceneName));
    }

    public void DeathTransition()
    {
        DataPersistenceManager.Instance.ManagePlayerDeath();

        DataPersistenceManager.Instance.SetShouldLoadData(false);

        PersistentObjectSpawner.hasSpawned = false; //allow persistentobjects to spawn again when starting game

        LoadSceneAfterDeath();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return fader.ShortFade(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(fader.ShortFade(0f));
    }

    private IEnumerator LoadSceneLongFade(string sceneName)
    {
        yield return fader.LongFade(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(fader.LongFade(0f));
    }

    private void LoadSceneAfterDeath() //will continue to run after its object is destroyed
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
