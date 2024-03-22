using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryScreen : PlayerManagementScreen
{
    //[SerializeField] PlayerManagementTopBar topBar; //handles moving between screens and maintaining color state
    //description
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;

    List<InventorySlot> sortedInventorySlots = new List<InventorySlot>();

    //bool isEnabled = false;

    int hoveredChoiceIndex = 0;

    void Start()
    {
        CreateInventorySlotList();
        HideScreen();
    }

    public override void Update()
    {
        base.Update();

        if (topBar.CheckAnyBarButtonSelected())
        {
            SetItemDetailDefault();
            return;
        }

        if (EventSystem.current.currentSelectedGameObject != sortedInventorySlots[hoveredChoiceIndex].gameObject)
        {
            //if (topBar.CheckAnyBarButtonSelected())
            //{
            //    hoveredChoiceIndex = -1;
            //}
            SetActiveButtonIndex();
            SetItemDetailSection();
        }
    }

    private void SetUpItemsForDisplay()
    {
        List<ItemInstance> itemList = InventoryManager.Instance.ItemList;

        for (int x = 0; x < InventoryManager.Instance.MaxNumOfItems; x++)
        {
            if (x < InventoryManager.Instance.GetInventoryCurrentItemCount())
            {
                sortedInventorySlots[x].ItemInstanceInSlot = itemList[x];
            }
            else
            {
                sortedInventorySlots[x].RemoveItemInstanceInSlot();
            }

            sortedInventorySlots[x].SetUpIcon();
            sortedInventorySlots[x].SetUpItemName();
        }

        hoveredChoiceIndex = 0;
        EventSystem.current.SetSelectedGameObject(sortedInventorySlots[hoveredChoiceIndex].gameObject);
        SetItemDetailSection();
    }

    private void SetItemDetailSection()
    {
        if (hoveredChoiceIndex < InventoryManager.Instance.GetInventoryCurrentItemCount())
        {
            //itemName.text = sortedInventorySlots[hoveredChoiceIndex].RetrieveItemName();
            itemDescription.text = sortedInventorySlots[hoveredChoiceIndex].RetrieveItemDescription();
        }
        else
        {
            //itemName.text = "";
            SetItemDetailDefault();
        }
    }

    private void SetItemDetailDefault()
    {
        itemDescription.text = "";
    }

    private void CreateInventorySlotList()
    {
        List<InventorySlot> unsortedInventorySlots = GetComponentsInChildren<InventorySlot>().ToList();

        for (int i = 0; i < InventoryManager.Instance.MaxNumOfItems; i++)
        {
            sortedInventorySlots.Add(unsortedInventorySlots.Find(inventorySlot => inventorySlot.SlotIndex == i));
        }
    }

    private void SetActiveButtonIndex()
    {
        hoveredChoiceIndex = sortedInventorySlots.IndexOf(EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>());
        print(hoveredChoiceIndex);
    }

    public override void ShowScreen()
    {
        SetUpItemsForDisplay();

        base.ShowScreen();

        topBar.InventoryScreenOpened();
    }

    //public override void ShowScreen()
    //{
    //    SetUpItemsForDisplay();

    //    gameObject.SetActive(true);

    //    isEnabled = true;

    //    topBar.InventoryScreenOpened();
    //}
}
