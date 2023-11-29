using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarBackup : MonoBehaviour
{

    Point goal;

    public List<Vector3> GetPath(Point startPos, Point endPos)
    {
        if (GridManager.Instance.GetGridTile(endPos.X, endPos.Y).GridTileItem && GridManager.Instance.GetGridTile(endPos.X, endPos.Y).GridTileItem.WalkSpeed == 0)
            return null;
        if (startPos == endPos)
            return new List<Vector3>() { new Vector3 (startPos.X, 0, startPos.Y), };
        goal = endPos;
        List<Point> open = new List<Point>() { startPos };

        GridManager.Instance.GetGridTile(startPos.X, startPos.Y).AStarInfo.gCost = 0;
        GridManager.Instance.GetGridTile(startPos.X, startPos.Y).AStarInfo.fCost = GetDistanceToGoal(startPos);

        while (open.Count > 0)
        {
            //finds the point closest to goal in a fast way
            Point current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.fCost > GridManager.Instance.GetGridTile(open[i].X, open[i].Y).AStarInfo.fCost)
                    current = open[i];
            }
            open.Remove(current);

            //checks if we found goal
            if (current == goal)
                return ReconstructPath(current);

            //finds neigbours
            List<Point> neighbours = GridManager.Instance.GetGridTileNeiboursPoints(current);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.gCost + 10 < GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.gCost)
                {
                    if (!GridManager.Instance.IsWalkable(neighbours[i]))
                        continue;

                    GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.cameFrom = current;//new Point(GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.xCord, GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.yCord);
                    GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.gCost = GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.gCost + 10;
                    GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.fCost = GetDistanceToGoal(neighbours[i]);
                    if (!open.Contains(neighbours[i]))
                        open.Add(neighbours[i]);
                }
            }
        }
        GridManager.Instance.ResetAStarInfo();
        Debug.LogError("No route found");
        return null;
    }

    public List<Vector3> GetPath(Point startPos, Point endPos, Point tileToIgnore)
    {
        if (GridManager.Instance.GetGridTile(endPos.X, endPos.Y).GridTileItem && GridManager.Instance.GetGridTile(endPos.X, endPos.Y).GridTileItem.WalkSpeed == 0)
            return null;
        if (startPos == endPos)
            return new List<Vector3>() { new Vector3(startPos.X, 0, startPos.Y), };
        goal = endPos;
        List<Point> open = new List<Point>() { startPos };

        GridManager.Instance.GetGridTile(startPos.X, startPos.Y).AStarInfo.gCost = 0;
        GridManager.Instance.GetGridTile(startPos.X, startPos.Y).AStarInfo.fCost = GetDistanceToGoal(startPos);

        while (open.Count > 0)
        {
            //finds the point closest to goal in a fast way
            Point current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.fCost > GridManager.Instance.GetGridTile(open[i].X, open[i].Y).AStarInfo.fCost)
                    current = open[i];
            }
            open.Remove(current);

            //checks if we found goal
            if (current == goal)
                return ReconstructPath(current);

            //finds neigbours
            List<Point> neighbours = GridManager.Instance.GetGridTileNeiboursPoints(current);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.gCost + 10 < GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.gCost && tileToIgnore != neighbours[i])
                {
                    if (GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).GridTileItem != null && GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).GridTileItem.WalkSpeed == 0)
                        continue;

                    GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.cameFrom = current;//new Point(GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.xCord, GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.yCord);
                    GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.gCost = GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.gCost + 10;
                    GridManager.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.fCost = GetDistanceToGoal(neighbours[i]);
                    if (!open.Contains(neighbours[i]))
                        open.Add(neighbours[i]);
                }
            }
        }
        GridManager.Instance.ResetAStarInfo();
        return null;
    }

    private float GetDistanceToGoal(Point startPos)
    {
        return Vector3.Distance(GridManager.Instance.GridToWorld(startPos.X,startPos.Y), GridManager.Instance.GridToWorld(4, 4));
    }

    public List<Vector3> ReconstructPath(Point end)
    {
        List<Vector3> path = new List<Vector3>();
        Point current = goal;
        while (GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.cameFrom != new Point(-1, -1))
        {
            path.Add(GridManager.Instance.GetVisualTile(current.X, current.Y).transform.position);
            current = GridManager.Instance.GetGridTile(current.X, current.Y).AStarInfo.cameFrom;
        }
        path.Add(GridManager.Instance.GetVisualTile(current.X, current.Y).transform.position);
        path.Reverse();
        GridManager.Instance.ResetAStarInfo();
        return path;
    }
}