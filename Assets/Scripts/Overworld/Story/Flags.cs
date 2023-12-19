using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Flags : MonoBehaviour, IDataPersistence
{
    public abstract bool CheckCondition(string flag);
    public abstract void SetCondition(string flag);
    public abstract void LoadData();
    public abstract void SaveData();
}
