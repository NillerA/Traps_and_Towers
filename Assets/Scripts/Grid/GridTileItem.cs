using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridTileItem : ScriptableObject
{

    [Tooltip("0 = not walkable\nHigher number equals faster")]
    public int WalkSpeed = 0;
    [Tooltip("The prefab shown on the tile")]
    public GameObject ItemPrefab;

}