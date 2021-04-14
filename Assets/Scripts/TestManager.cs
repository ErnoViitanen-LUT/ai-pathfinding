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
    public int[] gridSize;
    public float[] borderSize;
    private int gridx;
    private int gridy;
    private float cellSize = 0.75f;
    [SerializeField] private MapData map;
    private GameObject[,] goArray;

    private SimpleGrid<PathNode> grid;

    [System.Serializable]
    public class MapData
    {
        public List<int> grid;
    }
    void Start()
    {
        gridy = gridSize[0];
        gridx = gridSize[1];
        pathfinding = new Pathfinding(gridx, gridy, cellSize);
        grid = pathfinding.GetGrid();

        map = new MapData();
        map.grid = new List<int>();
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
                square.name = "square " + x + "-" + y;
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
            playerPathfindingHandler.SetTargetPosition(mouseWorldPosition);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);

            goArray[x, y].SetActive(pathfinding.ToggleNode(x, y));

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
        string mapJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/map.json");
        map = JsonUtility.FromJson<MapData>(mapJson);


        for (int x = 0; x < goArray.GetLength(0); x++)
        {
            for (int y = 0; y < goArray.GetLength(1); y++)
            {

                goArray[x, y].SetActive(Convert.ToBoolean(map.grid[x + y]));
            }
        }
        Debug.Log("Loading... " + Application.persistentDataPath + "/map.json");
    }
    public void SaveIntoJson()
    {
        map.grid = new List<int>();
        for (int x = 0; x < goArray.GetLength(0); x++)
        {
            for (int y = 0; y < goArray.GetLength(1); y++)
            {
                map.grid.Add(Convert.ToInt32(goArray[x, y].activeSelf));
            }
        }

        string mapJson = JsonUtility.ToJson(map);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/map.json", mapJson);
        Debug.Log("Saving... " + Application.persistentDataPath + "/map.json");
    }
}
