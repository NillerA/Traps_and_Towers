using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{

    private Grid grid = new Grid();

    [SerializeField]
    private GameObject GrassTilePrefab, WaterTilePrefab;
    [SerializeField]
    public Map map;

    public void Awake()
    {
        if(map != null)
        {
            grid = map.grid;
        }
        else
        {
            grid.Tiles = new GridTile[10,10];
        }
    }

    public void Start()
    {
        GameObject gridHolder = new GameObject();
        gridHolder.name = "Grid";
        for (int x = 0; x < grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < grid.Tiles.GetLength(1); y++)
            {
                GameObject tile = Instantiate(GrassTilePrefab,gridHolder.transform);
                tile.transform.position = GridToWorld(x, y);
                tile.name = x.ToString() + "," + y.ToString();
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
}
