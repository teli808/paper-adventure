using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventTrigger : StoryTrigger
{
    [SerializeField] ItemData item;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] bool isReceiveItemEvent;

    private void OnEnable()
    {
        EventManager.Instance.OnDialogueOver += ItemEventComplete;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDialogueOver -= ItemEventComplete;
    }

    public void StartItemEvent()
    {
        if (isReceiveItemEvent)
        {
            StartItemObtainedDialogue();
        }
        else
        {
            RemoveItem();
        }
    }
    private void StartItemObtainedDialogue()
    {
        EventManager.Instance.ChangePlayerState(PlayerState.TALKING_TO_NPC);
        DialogueManager.Instance.EnterDialogueForItemEvent(inkJSON, item.ItemName);
        isPlaying = true;

        InventoryManager.Instance.AddItemFromItemData(item);
        //create new event that character subscribes to, creates halo
    }

    private void RemoveItem()
    {
        isPlaying = true;
        InventoryManager.Instance.DeleteUsingItemData(item);
        ItemEventComplete();
    }

    private void ItemEventComplete()
    {
        if (isPlaying)
        {
            SetHasBeenTriggered();
            SetFlag();
            isPlaying = false;
        }

        //Player control is given back via DialogueOver event
    }
}
