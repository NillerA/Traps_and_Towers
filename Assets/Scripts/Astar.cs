using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Astar : MonoBehaviour
{
    [SerializeField]
    private WorldGrid worldGrid;

    public List<Vector3Int> Tiles = new List<Vector3Int>();    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public List<Tile> FindPath(Tile startPoint, Tile endPoint)
    {
        List<Tile> openPathTiles = new List<Tile>();
        List<Tile> closedPathTiles = new List<Tile>();

        Tile currentTile = startPoint;

        currentTile.G = 0;
        currentTile.H = getPathCost(startPoint.position, endPoint.position);


        openPathTiles.Add(currentTile);
        
        while(openPathTiles.Count!=0)
        {
             currentTile = openPathTiles.First();
            foreach (var t in openPathTiles)
            {
                if (t.F < currentTile.F || t.F == currentTile.F && t.H < currentTile.H)
                {
                    currentTile = t;
                }
            }
            openPathTiles.Remove(currentTile);
            closedPathTiles.Add(currentTile);

            int g = currentTile.H + 1;

            if(closedPathTiles.Contains(endPoint))
            {
                break;

            }

            foreach (Tile adjacentTile in currentTile.neighborTiles)
            {

                if(adjacentTile.isObstacle)
                {
                    continue;
                }

                if(closedPathTiles.Contains(adjacentTile))
                {
                    continue;
                }

                if(!openPathTiles.Contains(adjacentTile))
                {
                    adjacentTile.G = g;

                    adjacentTile.H = getPathCost(adjacentTile.position, endPoint.position);
                    adjacentTile.Parent = currentTile;
                    openPathTiles.Add(adjacentTile);
                    
                }
                else if (adjacentTile.F>g+adjacentTile.H)
                {
                    adjacentTile.G = g;
                }

                

            }

        }

       

             List<Tile> path = new List<Tile>();
        currentTile = endPoint;

        while (currentTile != startPoint)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }
        path.Add(startPoint);
        path.Reverse();

        return path;
    }
    
    protected static int getPathCost(Vector3Int startPosition,Vector3Int targetPosition )
    {
        int result = Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x), Mathf.Abs(startPosition.y - targetPosition.y), Mathf.Abs( startPosition.z - targetPosition.z));

        return result;
           

    }
    
}

public class Tile
{

    public Vector3Int position;

    public Tile Parent;
  
    public Tile(Vector3 pos)
    {
        position = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);

    }

    public int G;
    public int H;

    public int F => G + H;

    public List<Tile> neighborTiles = new List<Tile>();

    public bool isObstacle;

}
