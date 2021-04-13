using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private SimpleGrid<PathNode> grid;
    public int x;
    public int y;
    public bool isWalkable;
    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode previousNode;

    public PathNode(SimpleGrid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = true;
    }

    public void ToggleWalkability()
    {
        Debug.Log("toggled" + x + "," + y);
        isWalkable = !isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
