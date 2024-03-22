using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private List<IDataPersistence> dataPersistenceObjects;

    public static DataPersistenceManager Instance { get; private set; }

    [SerializeField] string startingSceneName = "TutorialLv1";
    [SerializeField] string mainMenuName = "MainMenu";
    [SerializeField] string battleSceneName = "BattleScene";

    [SerializeField] string easySaveFileName = "SaveFile.es3"; //Save needed for all other reasons, such as switching between scenes or between battle and overworld
    [SerializeField] string easySaveLastPlayerSave = "SaveFileLastPlayerSave.es3"; //Player initiated save, either from save block or auto save from StoryManager

    bool shouldLoad = true;

    private void Awake()
    {
        if (Instance != null)
        {
            print("Found more than one Data Persistence Manager in the scene");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (CheckGameFileExists()) CreatePlayerSaveFile(); //Needed if player never hits a save block during their play session
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //string loadedSceneName = SceneManager.GetActiveScene().name;

        //if (loadedSceneName == mainMenuName || loadedSceneName == battleSceneName) return; //Don't load data into these scenes

        if (shouldLoad) LoadGame();
    }

    public void NewGame()
    {
        if (CheckGameFileExists()) //consider adding a "are you sure" message if there is already data
        {
            //Are you sure message?
            DeleteAllFiles();
        }

        SceneManager.LoadSceneAsync(startingSceneName);
    }

    public IEnumerator ContinueGame()
    {
        string sceneNameToLoad;

        string uniqueIdentifier = "SceneSwitcher";

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameToLoadInto")))
        {
            sceneNameToLoad = ES3.Load<string>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "sceneNameToLoadInto"));
        }
        else
        {
            print("Error, no scene name to load");
            sceneNameToLoad = startingSceneName;
        }

        SetShouldLoadData(true);

        yield return SceneManager.LoadSceneAsync(sceneNameToLoad);
    }

    public void LoadGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData();
        }

        print("Data persistent objects all loaded");
        //RestoreFromBackup();
    }

    public void SaveGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData();
        }
    }

    public void CreatePlayerSaveFile()
    {
        ES3.CopyFile(easySaveFileName, easySaveLastPlayerSave);
        //if (CheckGameFileExists()) //edge case is handled by autosaving in StoryManager
        //{
        //    ES3.CopyFile(easySaveFileName, easySaveBackupName);
        //}
        //else
        //{
        //    dataPersistenceObjects = FindAllDataPersistenceObjects();

        //    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //    {
        //        dataPersistenceObj.SaveData();
        //    }

        //    ES3.CopyFile(easySaveFileName, easySaveBackupName);
        //}
    }

    public void SetShouldLoadData(bool shouldLoad)
    {
        this.shouldLoad = shouldLoad;
    }

    public void ManagePlayerDeath()
    {
        RestoreFromPlayerSave();
    }

    public void RestoreFromPlayerSave()
    {
        if (CheckGameFileExists(easySaveLastPlayerSave))
        {
            ES3.DeleteFile(easySaveFileName);
            ES3.RenameFile(easySaveLastPlayerSave, easySaveFileName);
        }
    }

    private void OnApplicationQuit() //called before application quits
    {
        RestoreFromPlayerSave();

        if (CheckGameFileExists(easySaveLastPlayerSave)) ES3.DeleteFile(easySaveLastPlayerSave);

    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool CheckGameFileExists(string fileName = null)
    {
        if (fileName == null)
        {
            return ES3.FileExists(easySaveFileName);
        }
        else
        {
            return ES3.FileExists(fileName);
        }
    }

    private void DeleteAllFiles()
    {
        ES3.DeleteFile(easySaveFileName);
        ES3.DeleteFile(easySaveLastPlayerSave);
    }
}
