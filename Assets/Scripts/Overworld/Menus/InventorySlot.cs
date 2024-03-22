using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [field: SerializeField] public int SlotIndex { get; private set; } //0 indexed

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemNameText;

    public ItemInstance ItemInstanceInSlot { get; set; }

    public void SetUpIcon()
    {
        if (ItemInstanceInSlot != null)
        {
            icon.sprite = ItemInstanceInSlot.ItemData.ItemIcon;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }

    public void SetUpItemName()
    {
        if (ItemInstanceInSlot != null)
        {
            itemNameText.text = RetrieveItemName();
        }
        else
        {
            itemNameText.text = "";
        }
    }

    public void RemoveItemInstanceInSlot()
    {
        ItemInstanceInSlot = null;
    }

    private string RetrieveItemName()
    {
        return ItemInstanceInSlot.ItemData.ItemName;
    }

    public string RetrieveItemDescription()
    {
        return ItemInstanceInSlot.ItemData.ItemDescription;
    }
}
