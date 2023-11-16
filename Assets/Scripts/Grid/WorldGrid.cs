using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGrid : MonoBehaviour, ISerializationCallbackReceiver
{

    [SerializeField]
    private Grid grid;
    [SerializeField, HideInInspector]
    private GameObject[,] gridTiles = new GameObject[0,0];
    [SerializeField, HideInInspector]
    private GameObject tileHolder;

    [SerializeField]
    private GameObject grassTilePrefab, waterTilePrefab;
    [SerializeField]
    public Map map;
    [SerializeField, Range(0,100)]
    private int xAmount, yAmount;


    public void Generate()
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
                    if(grid.Tiles != null && grid.Tiles.GetLength(0) > x && grid.Tiles.GetLength(1) > y && grid.Tiles[x,y] != null)
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
        gridTiles = new GameObject[grid.Tiles.GetLength(0), grid.Tiles.GetLength(1)];
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                gridTiles[x,y] = Instantiate(grassTilePrefab, GridToWorld(x, y), Quaternion.identity, tileHolder.transform);
                gridTiles[x, y].name = x.ToString() + "," + y.ToString();
                if (grid.Tiles[x,y].GridTileItem != null)
                {
                    Instantiate(grid.Tiles[x, y].GridTileItem.ItemPrefab, GridToWorld(x, y), Quaternion.identity, gridTiles[x, y].transform);
                }
                else if(gridTiles[x, y].transform.childCount > 1)
                {
                        DestroyImmediate(gridTiles[x, y].transform.GetChild(1).gameObject);
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

    public (int x, int y) WorldToGrid(Vector3 worldCoord)
    {
        float worldX = worldCoord.x;
        float worldY = worldCoord.z;
        int gridxFast;
        int gridYFast;
        worldY /= 0.75f;
        gridYFast = (int)worldY;
        if (gridYFast % 2 == 0)
            worldY -= 0.5f;
        gridxFast = (int)worldX;
        return (gridxFast, gridYFast);
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

    public GridTile GetGridTile(int x, int y)
    {
        return grid.Tiles[x, y];
    }

    public GameObject GetVisualTile(int x, int y)
    {
        Debug.Log(gridTiles.GetLength(0) + ", " + gridTiles.GetLength(1));
        return gridTiles[x, y];
    }

    public int GetXGridSize()
    {
        return grid.Tiles.GetLength(0);
    }

    public int GetYGridSize()
    {
        return grid.Tiles.GetLength(1);
    }



    [SerializeField]
    private GameObject[] serializedTiles;
    [SerializeField]
    private int serializedXAmount;

    public void OnAfterDeserialize()
    {
        gridTiles = new GameObject[serializedXAmount, serializedTiles.Count() / serializedXAmount];
        for (int x = 0; x < gridTiles.GetLength(0); ++x)
        {
            for (int y = 0; y < gridTiles.GetLength(1); y++)
            {
                gridTiles[x, y] = serializedTiles[x * gridTiles.GetLength(0) + y];
            }
        }
    }

    public void OnBeforeSerialize()
    {
        serializedTiles = new GameObject[gridTiles.Length];
        serializedXAmount = gridTiles.GetLength(0);
        for (int x = 0; x < gridTiles.GetLength(0); ++x)
        {
            for (int y = 0; y < gridTiles.GetLength(1); y++)
            {
                serializedTiles[x * gridTiles.GetLength(0) + y] = gridTiles[x, y];
            }
        }
    }
}