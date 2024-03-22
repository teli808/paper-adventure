using Ink;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SaveId))]
public class PlayerStatsManager : MonoBehaviour, IDataPersistence
{
    public static PlayerStatsManager Instance { get; private set; }

    [SerializeField] ProgressionData progressionData;

    Dictionary<Stat, int> currentStatsDict;

    public int CurrentLevel { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            print("Multiple PlayerStat managers exist");
        }

        Instance = this;

        progressionData.InitializeStatsProgressionDict();
        InitializeNewGameStats();
    }

    public void InitializeNewGameStats()
    {
        currentStatsDict = new Dictionary<Stat, int>();

        foreach (Stat statEnum in Enum.GetValues(typeof(Stat)))
        {
            if (statEnum == Stat.EXPERIENCE)
            {
                currentStatsDict[statEnum] = 0;
            }
            else
            {
                currentStatsDict[statEnum] = GetBaseStatByLevel(statEnum, 1);
            }
        }

        CurrentLevel = 1;
    }

    public void AddExperiencePoints(int experienceToAdd) //Multiplier if necessary
    {
        currentStatsDict[Stat.EXPERIENCE] += experienceToAdd;
    }

    public bool CheckLevelUp()
    {
        if (currentStatsDict[Stat.EXPERIENCE] >= GetBaseStatByLevel(Stat.EXPERIENCE, CurrentLevel))
        {
            int newExperience = currentStatsDict[Stat.EXPERIENCE] - GetBaseStatByLevel(Stat.EXPERIENCE, CurrentLevel);

            CurrentLevel += 1;

            //Cap experience to 1 point below next level up, if there is overflow. There should not be more than one level up event
            currentStatsDict[Stat.EXPERIENCE] = (int)Mathf.Clamp(newExperience, 0f, GetBaseStatByLevel(Stat.EXPERIENCE, CurrentLevel) - 1);

            //Replenish health/energy to max amount
            currentStatsDict[Stat.HEALTH] = GetBaseStatByLevel(Stat.HEALTH, CurrentLevel);
            currentStatsDict[Stat.ENERGY] = GetBaseStatByLevel(Stat.ENERGY, CurrentLevel);


            return true;
        }

        return false;
    }

    public int CalculateMaxStatIncrease(Stat stat) //Provides max stat increase between current level and previous level
    {
        if (CurrentLevel == 1)
        {
            Debug.Log("Cannot find max stat increase at level 1");
            return 0;
        }

        return GetBaseStatByLevel(stat, CurrentLevel) - GetBaseStatByLevel(stat, CurrentLevel - 1);
    }

    public int GetCurrentStat(Stat stat)
    {
        return currentStatsDict[stat];
    }

    public int GetBaseStatByLevel(Stat stat, int level)
    {
        return progressionData.GetBaseStatByLevel(stat, level);
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "currentStatsDict")))
        {
            currentStatsDict = ES3.Load<Dictionary<Stat, int>>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "currentStatsDict"));
            CurrentLevel = ES3.Load<int>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "CurrentLevel"));
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "currentStatsDict"), currentStatsDict);
        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "CurrentLevel"), CurrentLevel);
    }
}
