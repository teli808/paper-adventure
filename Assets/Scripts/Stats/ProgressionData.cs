using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Paper Adventure/LevelData")]
public class ProgressionData : ScriptableObject
{
    [SerializeField] StatProgression[] levelArray;

    Dictionary<Stat, int[]> statsProgressionDict;

    public void InitializeStatsProgressionDict()
    {
        statsProgressionDict = new Dictionary<Stat, int[]>();

        foreach (StatProgression statProgression in levelArray)
        {
            statsProgressionDict[statProgression.stat] = statProgression.statValuePerLevel;
        }
    }

    public int GetBaseStatByLevel(Stat stat, int level)
    {
        return statsProgressionDict[stat][level - 1];
    }
}
