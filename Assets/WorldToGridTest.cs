using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToGridTest : MonoBehaviour
{

    public WorldGrid grid;
    GameObject current;

    void Update()
    {
        (int gridX, int gridY) = grid.WorldToGrid(transform.position);
        Debug.Log(gridX + ", " + gridY);
        if (gridX > 0 && gridY > 0 && gridX < 10 && gridY < 10)
            if (current != grid.GetTileVisual(gridX, gridY))
            {
                if (current != null)
                    current.transform.GetChild(0).GetComponent<Material>().color = Color.green;
                current = grid.GetTileVisual(gridX, gridY);
                current.transform.GetChild(0).GetComponent<Material>().color = Color.white;
            }
    }
}
