using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
    }

    //Player Controller Events
    public event Action<PlayerState> OnChangePlayerState;
    public void ChangePlayerState(PlayerState playerState)
    {
        OnChangePlayerState?.Invoke(playerState);
    }

    public event Action<Transform> OnSpawnPlayerAtPosition;
    public void SpawnPlayerAtPosition(Transform transform)
    {
        OnSpawnPlayerAtPosition?.Invoke(transform);
    }

    public event Action OnFaderComplete;
    public void FaderComplete()
    {
        OnFaderComplete?.Invoke();
    }

    public event Action<Transform> OnOverworldDeath;
    public void OverworldDeath(Transform respawnPoint)
    {
        OnOverworldDeath?.Invoke(respawnPoint);
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
