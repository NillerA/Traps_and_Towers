using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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

public class AStarInfo
{
    public int gCost = int.MaxValue;
    public int fCost = int.MaxValue;
    public GridTile cameFrom;
    public int xCord, yCord;
}