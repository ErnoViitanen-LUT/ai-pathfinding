using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SimpleGrid that can be used for many things
    Credits:
    Grid Creation & Debug - Code Monkey
*/
public class SimpleGrid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private int offset_x; // world-space x-offset
    private int offset_y; // world-space y-offset
    private TGridObject[,] gridArray; // grid values

    /* The Func is a constructor that takes a grid (in this case this very object), and x and y)*/
    public SimpleGrid(int width, int height, float cellSize, Func<SimpleGrid<TGridObject>, int, int, TGridObject> createGridObject, int offset_x = 0, int offset_y = 0)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset_x = offset_x;
        this.offset_y = offset_y;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //create an array of gridObjects with the constructor provided in this class' constructor.
                gridArray[x, y] = createGridObject(this, x, y);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + new Vector3(offset_x, offset_y);
    }

    public void SetGridObject(int x, int y, TGridObject gridObject)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = gridObject;
            Debug.Log("value at " + x + ", " + y + " set");
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject gridObject)
    {
        int x, y;
        GetGridPosition(worldPosition, out x, out y);
        SetGridObject(x, y, gridObject);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return default(TGridObject);
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - offset_x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.y - offset_y) / cellSize);
    }

    public int GetGridWidth()
    {
        return width;
    }

    public int GetGridHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }
}
