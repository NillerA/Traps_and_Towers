using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{

    [SerializeField]
    private Grid grid = new Grid();
    private GameObject[,] visualTiles = new GameObject[0,0];
    private GameObject tileHolder;

    [SerializeField]
    private GameObject GrassTilePrefab, WaterTilePrefab;
    [SerializeField]
    public Map map;
    [SerializeField, Range(0,100)]
    private int xAmount, yAmount;

    public void OnValidate()
    {
        if(map != null)
        {
            grid = map.grid;
        }
        else
        {
            GridTile[,] newTiles = new GridTile[xAmount, yAmount];
            for(int x = 0; x < newTiles.GetLength(0); x++)
            {
                for (int y = 0; y < newTiles.GetLength(1); y++)
                {
                    if(grid.Tiles.GetLength(0) > x && grid.Tiles.GetLength(1) > y)
                        newTiles[x,y] = grid.Tiles[x,y];
                    else
                        newTiles[x,y] = new GridTile();
                }
            }
            grid.Tiles = newTiles;
        }

        if (tileHolder == null)
        {
            tileHolder = new GameObject();
            tileHolder.name = "Grid";
        }
        foreach(Transform tile in tileHolder.transform)
        {
            if (tile != null)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(tile.gameObject);
                };
            }
        }
        visualTiles = new GameObject[grid.Tiles.GetLength(0), grid.Tiles.GetLength(1)];
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                visualTiles[x,y] = Instantiate(GrassTilePrefab, GridToWorld(x, y), Quaternion.identity, tileHolder.transform);
                visualTiles[x, y].name = x.ToString() + "," + y.ToString();
                if (grid.Tiles[x,y].GridTileItem != null)
                {
                    Instantiate(grid.Tiles[x, y].GridTileItem.ItemPrefab, GridToWorld(x, y), Quaternion.identity, visualTiles[x, y].transform);
                }
                else if(visualTiles[x, y].transform.childCount > 1)
                {
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        DestroyImmediate(visualTiles[x, y].transform.GetChild(1).gameObject);
                    };
                }
            }
        }
    }

    public Vector3 GridToWorld(int x, int y)
    {
        if (y % 2 == 0)
            return new Vector3(x * 1 + 0.5f, 0, y * 0.75f);
        else
            return new Vector3(x * 1, 0, y * 0.75f);
    }

    public bool PlaceTileItem(int x, int y, GridTileItem item)
    {
        if(grid.Tiles[x, y] == null)
        {
            grid.Tiles[x,y].GridTileItem = item;
            return true;
        }
        else
            return false;
    }

    public void SetTileType(int x, int y, GridTile.TileTypes tileType)
    {
        grid.Tiles[x, y].TileType = tileType;
    }

    //private void OnDrawGizmos()
    //{
    //    for (int x = 0; x < grid.Tiles.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < grid.Tiles.GetLength(1); y++)
    //        {
    //            Gizmos.DrawMesh(exampleTile, GridToWorld(x, y),Quaternion.Euler(-90, 0, 0));
    //        }
    //    }
    //}
}
