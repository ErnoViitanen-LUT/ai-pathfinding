using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public PlayerPathfindingHandler playerPathfindingHandler;
    private Pathfinding pathfinding;
    private int gridx = 12;
    private int gridy = 12;
    private float cellSize = 0.75f;
    void Start()
    {
        pathfinding = new Pathfinding(gridx, gridy, cellSize);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);
            Debug.Log("loppupiste: (" + x + "," + y + ")");
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.Log("path " + i + ": " + path[i].x + ", " + path[i].y);
                    //Debug.DrawLine(new Vector3(path[i].x, path[i].y) + Vector3.one * 0.5f, new Vector3(path[i + 1].x, path[i + 1].y) + Vector3.one * 0.5f, Color.green, 10f);
                }
            }
            playerPathfindingHandler.SetTargetPosition(mouseWorldPosition);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);
            pathfinding.ToggleNode(x, y);
        }
    }
}
