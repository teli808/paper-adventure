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

    [SerializeField] string easySaveFileName = "SaveFile.es3";
    [SerializeField] string easySaveBackupName = "SaveFileBackup.es3";

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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string loadedSceneName = SceneManager.GetActiveScene().name;

        if (loadedSceneName == mainMenuName || loadedSceneName == battleSceneName) return; //Don't load data into these scenes

        LoadGame();
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

    public void ContinueGame()
    {
        string sceneNameToLoad;

        string uniqueIdentifer = "SceneSwitcher";

        if (ES3.KeyExists(uniqueIdentifer))
        {
            sceneNameToLoad = ES3.Load<string>(uniqueIdentifer);
        }
        else
        {
            print("Error, no scene name to load");
            sceneNameToLoad = startingSceneName;
        }

        SceneManager.LoadSceneAsync(sceneNameToLoad);
    }

    public void LoadGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData();
        }

        EventManager.Instance.DataLoadComplete();

        RestoreFromBackup();
    }

    public void SaveGame(bool shouldCreateBackup = false)
    {
        if (shouldCreateBackup) BackupFile();

        dataPersistenceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData();
        }
    }

    public void ManagePlayerDeath()
    {
        RestoreFromBackup();
    }

    public void BackupFile()
    {
        if (CheckGameFileExists()) //edge case is handled by autosaving in StoryManager
        {
            ES3.CopyFile(easySaveFileName, easySaveBackupName);
        }
    }

    public void RestoreFromBackup()
    {
        if (CheckGameFileExists(easySaveBackupName))
        {
            ES3.DeleteFile(easySaveFileName);
            ES3.RenameFile(easySaveBackupName, easySaveFileName);
        }
    }

    private void OnApplicationQuit() //called before application quits
    {
        RestoreFromBackup();
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
        ES3.DeleteFile(easySaveBackupName);
    }
}
