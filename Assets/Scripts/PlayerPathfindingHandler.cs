using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathfindingHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private const float speed = 30f;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {

        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
