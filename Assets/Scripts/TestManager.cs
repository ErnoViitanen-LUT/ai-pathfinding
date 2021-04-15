using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestManager : MonoBehaviour
{
    public PlayerPathfindingHandler playerPathfindingHandler;
    public GameObject player;
    public GameObject squarePrefab;

    private Pathfinding pathfinding;
    private SimpleGrid<PathNode> grid;

    private int gridx;
    private int gridy;
    private float cellSize = 0.75f;

    public int[] gridSize;
    public float[] borderSize;

    [SerializeField] private MapData map;
    private GameObject[,] squareArray;

    [System.Serializable]
    public class MapData
    {
        public Vector3 playerPos;
        public List<GridEntry> grid;
    }

    [System.Serializable]
    public class GridEntry
    {
        public int y;
        public int x;
        public bool w;

        public GridEntry(int x, int y, bool w)
        {
            this.x = x;
            this.y = y;
            this.w = w;
        }
    }

    void Start()
    {
        gridy = gridSize[0];
        gridx = gridSize[1];
        pathfinding = new Pathfinding(gridx, gridy, cellSize);
        grid = pathfinding.GetGrid();

        map = new MapData();
        map.grid = new List<GridEntry>();
        squareArray = new GameObject[gridx, gridy];

        player.transform.position = grid.GetWorldPosition(0, 0) + Vector3.one * cellSize * 0.5f;

        for (int x = 0; x < squareArray.GetLength(0); x++)
        {
            for (int y = 0; y < squareArray.GetLength(1); y++)
            {
                //create a matrix of squares the size of the grid.
                Vector3 squarePosition = grid.GetWorldPosition(x, y) + Vector3.one * cellSize * 0.5f;
                GameObject square = Instantiate(squarePrefab, squarePosition, Quaternion.identity);
                square.transform.localScale = new Vector3(cellSize - borderSize[0], cellSize - borderSize[1]);
                square.name = "square " + x + "-" + y;
                squareArray[x, y] = square;
            }
        }
        LoadFromJson();
    }

    private void Update()
    {
        // handle border toggle
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);
            // change background tile
            squareArray[x, y].SetActive(pathfinding.ToggleNode(x, y));

        } // handle movement
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);
            Debug.Log("loppupiste: (" + x + "," + y + ")");
            playerPathfindingHandler.SetTargetPosition(mouseWorldPosition, squareArray);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveIntoJson();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJson();
        }
    }

    public void LoadFromJson()
    {
        string file = "map.json"; // Application.persistentDataPath + "/map.json"
        string mapJson = System.IO.File.ReadAllText(file);
        map = JsonUtility.FromJson<MapData>(mapJson);

        player.transform.position = map.playerPos;
        foreach (GridEntry gridEntry in map.grid)
        {
            squareArray[gridEntry.x, gridEntry.y].SetActive(gridEntry.w);
            if (pathfinding.ToggleNode(gridEntry.x, gridEntry.y) != gridEntry.w)
            {
                pathfinding.ToggleNode(gridEntry.x, gridEntry.y);
            }
        }
        Debug.Log("Loading... " + Application.persistentDataPath + "/map.json");
    }

    public void SaveIntoJson()
    {
        map.grid = new List<GridEntry>();
        map.playerPos = player.transform.position;
        for (int x = 0; x < squareArray.GetLength(0); x++)
        {
            for (int y = 0; y < squareArray.GetLength(1); y++)
            {
                map.grid.Add(new GridEntry(x, y, squareArray[x, y].activeSelf));
            }
        }

        string mapJson = JsonUtility.ToJson(map);
        string file = "map.json"; // Application.persistentDataPath + "/map.json"
        System.IO.File.WriteAllText(file, mapJson);
        Debug.Log("Saving... " + Application.persistentDataPath + "/map.json");
    }
}
