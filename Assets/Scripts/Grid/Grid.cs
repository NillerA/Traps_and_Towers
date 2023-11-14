using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{

    [Tooltip("tiles")]
    public GridTile[,] Tiles = new GridTile[1,1];

}

public class GridTile
{

    public enum TileTypes
    {
        grass,
        water
    }
    [Tooltip("type of tile")]
    public TileTypes TileType;
    /// <summary>
    /// items on the tile like a mountain, forest or tower
    /// </summary>
    public GridTileItem GridTileItem;

}

public abstract class GridTileItem
{

    [Tooltip("0 = not walkable\nHigher number equals faster")]
    public int WalkSpeed = 0;
    [Tooltip("The prefab shown on the tile")]
    public GameObject ItemPrefab;

}