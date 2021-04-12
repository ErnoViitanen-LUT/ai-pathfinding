using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private SimpleGrid grid;
    private
    void Start()
    {
        grid = new SimpleGrid(8, 4, 1.5f, 3);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValueAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), 30); // test for setting values if needed
        }
    }


}
