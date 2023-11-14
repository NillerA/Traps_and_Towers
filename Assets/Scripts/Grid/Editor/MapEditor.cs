using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditor : MonoBehaviour
{

    //WorldGrid worldGrid;
    //[SerializeField]
    //int gridSizeX, gridSizeY;

    //void Awake()
    //{
    //    worldGrid = GetComponent<WorldGrid>();
    //    Debug.Log(worldGrid.map.grid.Tiles.Length);
    //    if (worldGrid.map.grid.Tiles.GetLength(0) != gridSizeX || worldGrid.map.grid.Tiles.GetLength(1) != gridSizeY)
    //    {
    //        GridTile[,] newTiles = new GridTile[gridSizeX, gridSizeY];
    //        for (int x = 0; x < gridSizeX; x++)
    //        {
    //            for (int y = 0; y < gridSizeY; y++)
    //            {
    //                if (x < worldGrid.map.grid.Tiles.GetLength(0) && y < worldGrid.map.grid.Tiles.GetLength(1))
    //                    newTiles[x, y] = worldGrid.map.grid.Tiles[x, y];
    //                else
    //                    newTiles[x, y] = new GridTile();
    //            }
    //        }
    //        worldGrid.map.grid.Tiles = newTiles;
    //    }
    //}

    //public void Save()
    //{
    //    EditorUtility.SetDirty(worldGrid.map);
    //    Debug.Log(worldGrid.map.grid.Tiles.Length);
    //}
}