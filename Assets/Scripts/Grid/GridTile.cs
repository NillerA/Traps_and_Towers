using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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