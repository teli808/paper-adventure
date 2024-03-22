using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleInfo", menuName = "Paper Adventure/BattleInfo", order = 0)]
public class BattleInfo : ScriptableObject
{
    const int numberOfUnits = 4;

    [field: SerializeField] public Unit PlayerOverride { get; private set; }

    [field: SerializeField] public Unit[] EnemyUnits { get; private set; } = new Unit[numberOfUnits]; 
}

