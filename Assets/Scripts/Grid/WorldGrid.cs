using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGrid : MonoBehaviour, ISerializationCallbackReceiver
{

    public static WorldGrid Instance;

    [SerializeField]
    private Grid grid;
    [SerializeField, HideInInspector]
    private GameObject[,] visualTiles = new GameObject[0,0];
    [SerializeField, HideInInspector]
    private GameObject tileHolder;

    [SerializeField]
    private GameObject grassTilePrefab, waterTilePrefab;
    [SerializeField]
    public Map map;
    [SerializeField, Range(0,100)]
    private int xAmount, yAmount;

    WorldGrid()
    {
        Instance = this;
    }

    public void Awake()
    {
        Instance = this;
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                grid.Tiles[x, y].AStarInfo = new AStarInfo();
                grid.Tiles[x, y].AStarInfo.xCord = x;
                grid.Tiles[x, y].AStarInfo.yCord = y;
                grid.Tiles[x, y].AStarInfo.cameFrom = new Point(-1,-1);
            }
        }
    }

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
        visualTiles = new GameObject[grid.Tiles.GetLength(0), grid.Tiles.GetLength(1)];
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                visualTiles[x,y] = Instantiate(grassTilePrefab, GridToWorld(x, y), Quaternion.identity, tileHolder.transform);
                visualTiles[x, y].name = x.ToString() + "," + y.ToString();
                if (grid.Tiles[x,y].GridTileItem != null)
                {
                    Instantiate(grid.Tiles[x, y].GridTileItem.ItemPrefab, GridToWorld(x, y), Quaternion.identity, visualTiles[x, y].transform);
                }
                else if(visualTiles[x, y].transform.childCount > 1)
                {
                        DestroyImmediate(visualTiles[x, y].transform.GetChild(1).gameObject);
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
        if(x >= 0 && x < GetXGridSize() && y >= 0 && y < GetYGridSize() && grid.Tiles[x, y].GridTileItem == null)
        {
            grid.Tiles[x,y].GridTileItem = item;
            Instantiate(item.ItemPrefab, visualTiles[x,y].transform.position, visualTiles[x,y].transform.rotation, visualTiles[x,y].transform);
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

    public GridTile[,] GetGridTiles() 
    { 
        return grid.Tiles; 
    }

    public List<Point> GetGridTileNeiboursPoints(Point pointToFindNeibourOf)
    {
        List<Point> neighbours = new List<Point>();
        if (pointToFindNeibourOf.X > 0)
            neighbours.Add(pointToFindNeibourOf - new Point(1, 0));
        if (pointToFindNeibourOf.X < GetXGridSize() - 1)
            neighbours.Add(pointToFindNeibourOf + new Point(1, 0));
        if (pointToFindNeibourOf.Y > 0)
            neighbours.Add(pointToFindNeibourOf - new Point(0, 1));
        if (pointToFindNeibourOf.Y < GetYGridSize() - 1)
            neighbours.Add(pointToFindNeibourOf + new Point(0, 1));
        if(pointToFindNeibourOf.X%2 == 0)
        {
            if(pointToFindNeibourOf.X > GetXGridSize() - 1)
            {
                if (pointToFindNeibourOf.Y > 0)
                    neighbours.Add(pointToFindNeibourOf - new Point(-1, 1));
                if (pointToFindNeibourOf.Y < GetYGridSize() - 1)
                    neighbours.Add(pointToFindNeibourOf + new Point(1, 1));
            }
        }
        else
        {
            if (pointToFindNeibourOf.X > 0)
            {
                if (pointToFindNeibourOf.Y > 0)
                neighbours.Add(pointToFindNeibourOf - new Point(1, 1));
                if (pointToFindNeibourOf.Y < GetYGridSize() - 1)
                    neighbours.Add(pointToFindNeibourOf + new Point(-1, 1));
            }
        }
        return neighbours;
    }

    public void ResetAStarInfo()
    {
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                grid.Tiles[x, y].AStarInfo = new AStarInfo();
                grid.Tiles[x, y].AStarInfo.xCord = x;
                grid.Tiles[x, y].AStarInfo.yCord = y;
                grid.Tiles[x, y].AStarInfo.cameFrom = new Point(-1, -1);
            }
        }
    }

    public GameObject GetVisualTile(int x, int y)
    {
        return visualTiles[x, y];
    }

    public GameObject[,] GetVisualTiles() 
    {
        return visualTiles;
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
        visualTiles = new GameObject[serializedXAmount, serializedTiles.Count() / serializedXAmount];
        for (int x = 0; x < visualTiles.GetLength(0); ++x)
        {
            for (int y = 0; y < visualTiles.GetLength(1); y++)
            {
                visualTiles[x, y] = serializedTiles[x * visualTiles.GetLength(0) + y];
            }
        }
    }

    public void OnBeforeSerialize()
    {
        serializedTiles = new GameObject[visualTiles.Length];
        serializedXAmount = visualTiles.GetLength(0);
        for (int x = 0; x < visualTiles.GetLength(0); ++x)
        {
            for (int y = 0; y < visualTiles.GetLength(1); y++)
            {
                serializedTiles[x * visualTiles.GetLength(0) + y] = visualTiles[x, y];
            }
        }
    }
}