using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour 
{
    public FMODEventsBattle Battle { get; private set; }
    public FMODEventsOW Overworld { get; private set; }
    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene");
        }

        Instance = this;

        Battle = GetComponent<FMODEventsBattle>();
        Overworld = GetComponent<FMODEventsOW>();
    }
}
