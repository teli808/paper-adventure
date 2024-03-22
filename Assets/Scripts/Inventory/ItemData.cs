using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [field: SerializeField] public string ItemName { get; set; }
    [field: SerializeField] public Sprite ItemIcon { get; set; }
    //[field: SerializeField] public GameObject ItemModel { get; set; }
    [field: SerializeField] public string ItemDescription { get; set; }

}
