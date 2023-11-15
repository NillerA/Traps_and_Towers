using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Grid : ISerializationCallbackReceiver
{

    [SerializeField, Tooltip("tiles")]
    public GridTile[,] Tiles;

    [SerializeField]
    private GridTile[] serializedTiles;
    [SerializeField]
    private int serializedXAmount;

    public void OnAfterDeserialize()
    {
        Tiles = new GridTile[serializedXAmount, serializedTiles.Count() / serializedXAmount];
        for (int x = 0; x < Tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                Tiles[x, y] = serializedTiles[x * Tiles.GetLength(0) + y];
            }
        }
    }

    public void OnBeforeSerialize()
    {
        serializedTiles = new GridTile[Tiles.Length];
        serializedXAmount = Tiles.GetLength(0);
        for (int x = 0; x < Tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                serializedTiles[x * Tiles.GetLength(0) + y] = Tiles[x, y];
            }
        }
    }
}