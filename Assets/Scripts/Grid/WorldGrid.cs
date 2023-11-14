using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{

    private Grid grid = new Grid();
    private GameObject[,] visualTiles = new GameObject[0,0];
    private GameObject tileHolder;

    [SerializeField]
    private GameObject GrassTilePrefab, WaterTilePrefab;
    [SerializeField]
    public Map map;
    [SerializeField, Range(1,100)]
    private int xAmount, yAmount;

    public void OnValidate()
    {
        if(map != null)
        {
            grid = map.grid;
        }
        else
        {
            grid.Tiles = new GridTile[xAmount,yAmount];
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

        if (tileHolder == null)
        {
            tileHolder = new GameObject();
            tileHolder.name = "Grid";
        }
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                visualTiles[x,y] = Instantiate(GrassTilePrefab, GridToWorld(x, y), Quaternion.identity, tileHolder.transform);
                visualTiles[x, y].name = x.ToString() + "," + y.ToString();
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
