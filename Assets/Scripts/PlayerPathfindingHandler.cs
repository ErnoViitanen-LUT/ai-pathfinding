using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathfindingHandler : MonoBehaviour
{
    private const float speed = 10f;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private bool debugModeOn = false;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (Input.GetMouseButtonDown(2)) ToggleDebugMode();
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
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

    public void SetTargetPosition(Vector3 targetPosition, GameObject[,] squareArray = null)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        if (squareArray != null) DrawFCost(squareArray); //for debugging f-cost to grid
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void DrawFCost(GameObject[,] squareArray)
    {
        SimpleGrid<PathNode> grid = Pathfinding.Instance.GetGrid();
        for (int x = 0; x < squareArray.GetLength(0); x++)
        {
            for (int y = 0; y < squareArray.GetLength(1); y++)
            {
                GameObject fCostText = squareArray[x, y].transform.GetChild(0).gameObject;
                fCostText.SetActive(debugModeOn);
                if (debugModeOn)
                {
                    int fCost = grid.GetGridObject(x, y).fCost;
                    if (fCost > 1000 || fCost < 0)
                    {
                        fCostText.GetComponent<TextMesh>().text = "-";
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
        debugModeOn = !debugModeOn;
    }
}

