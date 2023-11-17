using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarBackup : MonoBehaviour
{

    Point goal;

    public List<Vector3> GetPath(Point startPos, Point endPos)
    {
        if (WorldGrid.Instance.GetGridTile(endPos.X, endPos.Y).GridTileItem)
            return null;
        if (startPos == endPos)
            return new List<Vector3>() { new Vector3 (startPos.X, 0, startPos.Y), };
        goal = endPos;
        List<Point> open = new List<Point>() { startPos };

        WorldGrid.Instance.GetGridTile(startPos.X, startPos.Y).AStarInfo.gCost = 0;
        WorldGrid.Instance.GetGridTile(startPos.X, startPos.Y).AStarInfo.fCost = GetDistanceToGoal(startPos);

        while (open.Count > 0)
        {
            //finds the point closest to goal in a fast way
            Point current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.fCost > WorldGrid.Instance.GetGridTile(open[i].X, open[i].Y).AStarInfo.fCost)
                    current = open[i];
            }
            open.Remove(current);

            //checks if w found goal
            if (current == goal)
                return ReconstructPath(current);

            //finds neigbours
            List<Point> neighbours = WorldGrid.Instance.GetGridTileNeiboursPoints(current);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.gCost + 10 < WorldGrid.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.gCost)
                {
                    if (WorldGrid.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).GridTileItem != null)
                        continue;
                    //if (current.X > neighbours[i].X && current.Y != neighbours[i].Y)
                    //{

                    //}
                    //else if (current.X < neighbours[i].X && current.Y != neighbours[i].Y)
                    //{

                    //}
                    //else if (current.X > neighbours[i].X)
                    //{
                    //    if (WorldGrid.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).GridTileItem != null)
                    //        continue;
                    //}
                    //else if (current.X < neighbours[i].X)
                    //{
                    //    if (_mazeCells[current.X, current.Y].Walls[1])
                    //        continue;
                    //}
                    //else if (current.Y > neighbours[i].Y)
                    //{
                    //    if (_mazeCells[current.X, current.Y - 1].Walls[0])
                    //        continue;
                    //}
                    //else if (current.Y < neighbours[i].Y)
                    //{
                    //    if (_mazeCells[current.X, current.Y].Walls[0])
                    //        continue;
                    //}
                    WorldGrid.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.cameFrom = new Point(WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.xCord, WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.yCord);
                    WorldGrid.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.gCost = WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.gCost + 10;
                    WorldGrid.Instance.GetGridTile(neighbours[i].X, neighbours[i].Y).AStarInfo.fCost = GetDistanceToGoal(neighbours[i]);
                    if (!open.Contains(neighbours[i]))
                        open.Add(neighbours[i]);
                }
            }
        }
        Debug.LogError("No route found");
        return null;
    }

    private int GetDistanceToGoal(Point startPos)
    {
        int distance = 0;
        int currentX = startPos.X, currentY = startPos.Y;
        while (currentX != goal.X)
        {
            distance += 10;
            if (currentX < goal.X)
                currentX++;
            else
                currentX--;
        }
        while (currentY != goal.Y)
        {
            distance += 10;
            if (currentY < goal.Y)
                currentY++;
            else
                currentY--;
        }
        return distance;
    }

    public List<Vector3> ReconstructPath(Point end)
    {
        List<Vector3> path = new List<Vector3>();
        Point current = goal;
        while (WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.cameFrom != null)
        {
            path.Add(WorldGrid.Instance.GetVisualTile(current.X, current.Y).transform.position + new Vector3(0.5f, 0.5f));
            current = WorldGrid.Instance.GetGridTile(current.X, current.Y).AStarInfo.cameFrom;
        }
        path.Reverse();
        return path;
    }
}
