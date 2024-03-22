using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlags : Flags, IDataPersistence
{
    Dictionary<string, bool> flagsEnumDict;

    private void Awake()
    {
        flagsEnumDict = new Dictionary<string, bool>();

        foreach (TutorialFlagsEnum name in Enum.GetValues(typeof(TutorialFlagsEnum)))
        {
            flagsEnumDict[name.ToString()] = false;
        }
    }

    public override bool CheckCondition(string flag)
    {
        return flagsEnumDict[flag];
    }

    public override void SetCondition(string flag)
    {
        flagsEnumDict[flag] = true;
    }

    public override void LoadData()
    {
        string uniqueIdentifier = "TutorialFlags";

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "flagsEnumDict")))
        {
            flagsEnumDict = ES3.Load<Dictionary<string, bool>>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "flagsEnumDict"));
        }
    }

    public override void SaveData()
    {
        string uniqueIdentifier = "TutorialFlags";

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "flagsEnumDict"), flagsEnumDict);

    }
}
