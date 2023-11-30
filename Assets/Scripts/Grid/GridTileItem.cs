using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridTileItem : ScriptableObject
{

    [Range(0,10),Tooltip("0 = not walkable\nHigher number equals faster")]
    public float WalkSpeed = 0;
    [Tooltip("The prefab shown on the tile")]
    public GameObject ItemPrefab;
    [Tooltip("a short description of the tile")]
    public string description = "description of Tile";

}