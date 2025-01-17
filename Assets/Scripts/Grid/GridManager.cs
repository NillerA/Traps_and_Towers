using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GridManager : MonoBehaviour, ISerializationCallbackReceiver
{

    public static GridManager Instance;

    [SerializeField]
    private GridTileItem tileItemToIgnore;


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

    GridManager()
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
                visualTiles[x,y].transform.GetChild(0).GetComponent<Renderer>().material.DisableKeyword("_EMISSION");//turns emmision off for the tile material
            }
        }
    }

#if UNITY_EDITOR
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
#endif

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
        if (gridYFast % 2 != 0)
            worldX += 0.5f;
        gridxFast = (int)worldX;
        if (gridxFast >= 0 && gridxFast < GetXGridSize() && gridYFast >= 0 && gridYFast < GetYGridSize())
        {
            List<Point> gridPoints = GetGridTileNeiboursPoints(new Point(gridxFast,gridYFast));
            Point closestPoint = new Point(gridxFast,gridYFast);
            float closestDistance = Vector3.Distance(worldCoord, visualTiles[closestPoint.X,closestPoint.Y].transform.position);
            foreach (Point p in gridPoints)
            {
                float dist = Vector3.Distance(worldCoord, visualTiles[p.X, p.Y].transform.position);
                if (dist < closestDistance)
                {
                    closestPoint = p;
                    closestDistance = dist;
                }
            }
            return (closestPoint.X, closestPoint.Y);
        }
        return (-1,-1);
    }

    public Point WorldToGridPoint(Vector3 worldCoord)
    {
        float worldX = worldCoord.x;
        float worldY = worldCoord.z;
        int gridxFast;
        int gridYFast;
        worldY /= 0.75f;
        gridYFast = (int)worldY;
        if (gridYFast % 2 != 0)
            worldX += 0.5f;
        gridxFast = (int)worldX;
        if (gridxFast >= 0 && gridxFast < GetXGridSize() && gridYFast >= 0 && gridYFast < GetYGridSize())
        {
            List<Point> gridPoints = GetGridTileNeiboursPoints(new Point(gridxFast, gridYFast));
            Point closestPoint = new Point(gridxFast, gridYFast);
            float closestDistance = Vector3.Distance(worldCoord, visualTiles[closestPoint.X, closestPoint.Y].transform.position);
            foreach (Point p in gridPoints)
            {
                float dist = Vector3.Distance(worldCoord, visualTiles[p.X, p.Y].transform.position);
                if (dist < closestDistance)
                {
                    closestPoint = p;
                    closestDistance = dist;
                }
            }
            return closestPoint;
        }
        return new Point(-1, -1);
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

    public bool RemoveTileItem(int x, int y)
    {
        if (x >= 0 && x < GetXGridSize() && y >= 0 && y < GetYGridSize() && grid.Tiles[x, y].GridTileItem == null)
            return false;
        else if (grid.Tiles[x, y].GridTileItem == GameManager.Instance.Base || grid.Tiles[x, y].GridTileItem == GameManager.Instance.monsterCave)
        {
            return false;
        }
        else
        {
            grid.Tiles[x, y].GridTileItem = null;
            Destroy(visualTiles[x, y].transform.GetChild(1).gameObject);
            return true;
        }
    }

    public bool CanPlaceItem(int x, int y)
    {
        if (x >= 0 && x < GetXGridSize() && y >= 0 && y < GetYGridSize() && grid.Tiles[x, y].GridTileItem == null)
            return true;
        else
            return false;
    }

    public bool IsWalkable(Point tile)
    {
        if (GetGridTile(tile.X, tile.Y).GridTileItem == null)
            return true;
        if(GetGridTile(tile.X, tile.Y).GridTileItem != null && GetGridTile(tile.X, tile.Y).GridTileItem.WalkSpeed == 0)
            return false;
        return true;
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
        if (pointToFindNeibourOf.Y % 2 == 0)
        {
            if (pointToFindNeibourOf.X < GetXGridSize() - 1)
            {
                if (pointToFindNeibourOf.Y > 0)
                    neighbours.Add(pointToFindNeibourOf + new Point(1, -1));
                if (pointToFindNeibourOf.Y < GetYGridSize() - 1)
                    neighbours.Add(pointToFindNeibourOf + new Point(1, 1));
            }
        }
        else
        {
            if (pointToFindNeibourOf.X > 0)
            {
                if (pointToFindNeibourOf.Y > 0)
                    neighbours.Add(pointToFindNeibourOf + new Point(-1, -1));
                if (pointToFindNeibourOf.Y < GetYGridSize() - 1)
                    neighbours.Add(pointToFindNeibourOf + new Point(-1, 1));
            }
        }
        return neighbours;
    }

    //TODO: implement radius(only takes 1 in radius at all times)
    public List<Vector3> GetTilesInRadius(Point location, int radius)
    {
        List<Vector3> tiles = new List<Vector3>();
        List<Point> tilePoints = GetGridTileNeiboursPoints(location);
        foreach (Point tilePoint in tilePoints)
        {
            tiles.Add(GridToWorld(tilePoint.X,tilePoint.Y));
        }
        return tiles;
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
        for (int x = 0; x < visualTiles.GetLength(0); x++)
        {
            for (int y = 0; y < visualTiles.GetLength(1); y++)
            {
                visualTiles[x, y] = serializedTiles[x * visualTiles.GetLength(1) + y];
            }
        }
    }

    public void OnBeforeSerialize()
    {
        serializedTiles = new GameObject[visualTiles.GetLength(0) * visualTiles.GetLength(1)];
        serializedXAmount = visualTiles.GetLength(0);
        for (int x = 0; x < visualTiles.GetLength(0); x++)
        {
            for (int y = 0; y < visualTiles.GetLength(1); y++)
            {
                serializedTiles[(x * visualTiles.GetLength(1)) + y] = visualTiles[x, y];
            }
        }
    }
}