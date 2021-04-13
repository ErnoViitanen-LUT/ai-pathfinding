using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; //rounded sqrt(2)*10
    private SimpleGrid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Pathfinding(int width, int height)
    {
        grid = new SimpleGrid<PathNode>(width, height, 1f, (SimpleGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public SimpleGrid<PathNode> GetGrid()
    {
        return grid;
    }

    public void ToggleNode(int x, int y)
    {
        GetNode(x, y).ToggleWalkability();
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final Node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                // if newly counted G-cost is lower than the one previously stored to neighbournode, update it with new one
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        Debug.Log("nothing found");
        // Out of nodes on the openList
        return null;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode> { endNode };
        PathNode currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y)); // left
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1)); // bottom-left 
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1)); // upper-right 
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y)); // right
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1)); // bottom-right
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1)); // upper-right
        }
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1)); // bottom
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1)); // top

        return neighbourList;
    }

    private PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }


}