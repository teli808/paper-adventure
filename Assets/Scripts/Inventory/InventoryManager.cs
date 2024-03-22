using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SaveId))]
public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public static InventoryManager Instance { get; private set; }
    public List<ItemInstance> ItemList { get; private set; } = new List<ItemInstance>();

    public int MaxNumOfItems { get; private set; } = 6;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Inventory Manager already exists");
        }
        Instance = this;
    }

    public void AddItemFromItemData(ItemData itemData)
    {
        ItemInstance newItem = new ItemInstance(itemData);

        if (ItemList.Count >= MaxNumOfItems)
        {
            Debug.Log("Bag is full");
            //Add functionality for case when bag is full
        }

        ItemList.Add(newItem);
    }

    public void DeleteUsingItemData(ItemData itemData)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemData == itemData)
            {
                ItemList.RemoveAt(i);
                return;
            }
        }

        print("Item not found to delete");
    }

    public int GetInventoryCurrentItemCount()
    {
        return ItemList.Count;
    }

    public void LoadData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        if (ES3.KeyExists(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "itemList")))
        {
            ItemList = ES3.Load<List<ItemInstance>>(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "itemList"));
        }
    }

    public void SaveData()
    {
        string uniqueIdentifier = GetComponent<SaveId>().GetUniqueIdentifier();

        ES3.Save(SaveKeyCreator.CreateFullKey(uniqueIdentifier, "itemList"), ItemList);
    }
}
