using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Credits
    Grid Creation & Debug - Code Monkey (Youtube)
*/
public class SimpleGrid
{
    private int width;
    private int height;
    private float cellSize;
    private int offset_x; // world-space x-offset
    private int offset_y; // world-space y-offset
    private int[,] gridArray; // grid values

    public SimpleGrid(int width, int height, float cellSize, int offset_x = 0, int offset_y = 0)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset_x = offset_x;
        this.offset_y = offset_y;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + new Vector3(offset_x, offset_y);
    }

    // maybe change to public later?
    private void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            Debug.Log("value at " + x + ", " + y + " set to " + value);
        }
    }

    public void SetValueAtPosition(Vector3 worldPosition, int value)
    {
        int x, y;
        GetGridPosition(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    private void GetGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - offset_x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.y - offset_y) / cellSize);
    }
}
