using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Event Manager in the scene");
        }
        Instance = this;
        print("EventManager setup");
    }

    //Data Management
    public event Action OnDataLoadComplete; //data can get overwritten if subscribing to SceneLoaded rather than this
    public void DataLoadComplete()
    {
        OnDataLoadComplete?.Invoke();
    }

    //Player Controller Events
    public event Action<PlayerState> OnChangePlayerState;
    public void ChangePlayerState(PlayerState playerState)
    {
        OnChangePlayerState?.Invoke(playerState);
    }

    //General Dialogue
    public event Action OnDialogueStarted;
    public void DialogueStarted()
    {
        OnDialogueStarted?.Invoke();
    }

    public event Action OnDialogueSubmitted;
    public void DialogueSubmitted()
    {
        OnDialogueSubmitted?.Invoke();
    }

    public event Action OnDialogueOver;
    public void DialogueOver()
    {
        OnDialogueOver?.Invoke();
    }

    //Battle System Events
    public event Action OnBattleSetUp;
    public void BattleSetUp()
    {
        OnBattleSetUp?.Invoke();
    }

    public event Action<BattleState> OnChangedBattleState;
    public void ChangedBattleState(BattleState battleState)
    {
        OnChangedBattleState?.Invoke(battleState);
    }
}
