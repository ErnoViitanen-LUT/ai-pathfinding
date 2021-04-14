using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathfindingHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private const float speed = 10f;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
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
                    // check if the way has been blocked mid-movement
                    PathNode node = Pathfinding.Instance.GetNode(pathVectorList[currentPathIndex]);
                    if (node != null && !node.isWalkable) StopMoving(targetPosition);
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

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
