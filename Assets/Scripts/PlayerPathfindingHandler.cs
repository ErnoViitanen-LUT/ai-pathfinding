using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathfindingHandler : MonoBehaviour
{
    private const float speed = 10f;
    private const float calcSpeed = 50f;
    private bool calcDrawOverride = false;
    private int currentPathIndex;
    private Vector3 targetPosition;
    private List<Vector3> pathVectorList;
    private bool halted = true;
    private List<PathNode> nodeList;
    private bool debugModeOn = true;

    private GameObject[,] squareArray;
    public Color pathColor = Color.green;
    public Color calcColor = Color.gray;
    private float calc = 0;

    // Update is called once per frame
    void Update()
    {
        if (halted) HandlePathDrawing();
        HandleMovement();
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space)) ToggleDebugMode();
    }

    private void HandlePathDrawing()
    {

        if (nodeList != null)
        {
            if (nodeList.Count > 0)
            {
                calc += calcSpeed * Time.deltaTime;
                if (calc >= 1f || calcDrawOverride)
                {
                    var item = nodeList[0];
                    GameObject go = squareArray[item.x, item.y].transform.GetChild(0).gameObject;
                    go.SetActive(true);
                    nodeList.RemoveAt(0);
                    calc = 0;
                }
            }
            else
            {
                var item = Pathfinding.Instance.GetNode(targetPosition);
                GameObject go = squareArray[item.x, item.y].transform.GetChild(0).gameObject;
                go.SetActive(true);
                go.GetComponent<TextMesh>().color = Color.red;
                calcDrawOverride = false;
                halted = false;
            }
        }

    }

    private void HandleMovement()
    {
        if (pathVectorList != null && !halted)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                PathNode currentNode = Pathfinding.Instance.GetNode(pathVectorList[currentPathIndex]);
                if (true) MarkPath(currentNode);

                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) StopMoving(targetPosition);
                else
                {
                    // check if the way has been blocked mid-movement, if so try to find another path
                    PathNode node = Pathfinding.Instance.GetNode(pathVectorList[currentPathIndex]);
                    if (node != null && !node.isWalkable) SetTargetPosition(pathVectorList[pathVectorList.Count - 1]);
                }
            }
        }
    }

    private void StopMoving(Vector3 forcedEndPosition)
    {
        pathVectorList = null;
        transform.position = forcedEndPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void MarkPath(PathNode currentPosition)
    {
        GameObject fCostText = squareArray[currentPosition.x, currentPosition.y].transform.GetChild(0).gameObject;
        TextMesh mesh = fCostText.GetComponent<TextMesh>();
        mesh.color = Color.black;
        mesh.fontStyle = FontStyle.Bold;
        //Debug.Log("Draw neighbors" + currentPosition.x + ":" + currentPosition.y);
        /*List<PathNode> neighbors = Pathfinding.Instance.GetNeighbourList(currentPosition);
        foreach (PathNode node in neighbors)
        {
            GameObject fCostText = sqArray[node.x, node.y].transform.GetChild(0).gameObject;
            fCostText.SetActive(true);
        }*/
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        debugModeOn = true;
        halted = true; // for drawing calculated path before actual movement
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        DrawFCost(); //for debugging f-cost to grid
        nodeList = new List<PathNode>(Pathfinding.Instance.drawList);

        this.targetPosition = targetPosition;

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
    public void SetSquareArray(GameObject[,] squareArray)
    {
        this.squareArray = squareArray;
    }

    public void DrawFCost()
    {
        SimpleGrid<PathNode> grid = Pathfinding.Instance.GetGrid();
        for (int x = 0; x < squareArray.GetLength(0); x++)
        {
            for (int y = 0; y < squareArray.GetLength(1); y++)
            {
                squareArray[x, y].GetComponent<SpriteRenderer>().color = Color.white;
                GameObject fCostText = squareArray[x, y].transform.GetChild(0).gameObject;
                fCostText.GetComponent<TextMesh>().color = calcColor;
                fCostText.SetActive(false);
                if (true)
                {
                    int fCost = grid.GetGridObject(x, y).fCost;
                    if (fCost > 1000 || fCost < 0)
                    {
                        fCostText.GetComponent<TextMesh>().text = "";
                    }
                    else
                    {
                        fCostText.GetComponent<TextMesh>().text = (fCost / 10f).ToString();
                    }

                }

            }
        }
    }

    private void ToggleDebugMode()
    {
        if (halted) calcDrawOverride = true;
        else
        {
            debugModeOn = !debugModeOn;
            for (int x = 0; x < squareArray.GetLength(0); x++)
            {
                for (int y = 0; y < squareArray.GetLength(1); y++)
                {
                    GameObject fCostText = squareArray[x, y].transform.GetChild(0).gameObject;
                    fCostText.SetActive(debugModeOn);
                }
            }
        }
    }
}

