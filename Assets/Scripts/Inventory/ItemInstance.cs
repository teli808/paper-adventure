using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class ItemInstance
{
    [field: SerializeField] public ItemData ItemData { get; private set; }

    public ItemInstance(ItemData itemData)
    {
        ItemData = itemData;
    }

    private void Update()
    {
        //if (ItemData.GetType() == typeof(StoryItemData))
        //{
        //    StoryItemData storyItemData = (StoryItemData)ItemData;
        //    print(storyItemData.storyItemTestField);
        //}
    }

    //handle different types of ItemData - consumables
}
