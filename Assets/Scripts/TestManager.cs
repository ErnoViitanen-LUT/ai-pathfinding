using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public PlayerPathfindingHandler playerPathfindingHandler;
    public GameObject player;
    public GameObject squarePrefab;
    private Pathfinding pathfinding;
    public int[] gridSize;
    public float[] borderSize;
    private int gridx;
    private int gridy;
    private float cellSize = 0.75f;

    private SimpleGrid<PathNode> grid;
    private GameObject[,] goArray;
    void Start()
    {
        gridy = gridSize[0];
        gridx = gridSize[1];
        pathfinding = new Pathfinding(gridx, gridy, cellSize);
        grid = pathfinding.GetGrid();

        goArray = new GameObject[gridx, gridy];

        player.transform.position = grid.GetWorldPosition(0, 0) + Vector3.one * cellSize * 0.5f;

        for (int x = 0; x < goArray.GetLength(0); x++)
        {
            for (int y = 0; y < goArray.GetLength(1); y++)
            {
                //create an array of gridObjects with the constructor provided in this class' constructor.

                Vector3 dd = grid.GetWorldPosition(x, y) + Vector3.one * cellSize * 0.5f;

                GameObject square = Instantiate(squarePrefab, dd, Quaternion.identity);
                square.transform.localScale = new Vector3(cellSize - borderSize[0], cellSize - borderSize[1]);
                //square.SetActive(false);
                goArray[x, y] = square;

            }
        }
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

            goArray[x, y].SetActive(pathfinding.ToggleNode(x, y));

        }
    }
}
