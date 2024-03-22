using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBlock : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;

    bool readyToSave = false;

    private void OnEnable()
    {
        EventManager.Instance.OnDialogueOver += OnSaveDialogueOver;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDialogueOver -= OnSaveDialogueOver;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventManager.Instance.ChangePlayerState(PlayerState.TALKING_TO_NPC);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Overworld.SaveBlockHit, transform.position);
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
            readyToSave = true;
        }
    }

    private void OnSaveDialogueOver()
    {
        if (readyToSave)
        {
            DataPersistenceManager.Instance.SaveGame();
            DataPersistenceManager.Instance.CreatePlayerSaveFile();
            readyToSave = false;
        }
    }

}
