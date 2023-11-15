using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private int health;
    [SerializeField]
    private WorldGrid worldGrid;
    [SerializeField]
    private GridTileItem Base;

    private void Awake()
    {
        if (worldGrid == null)
            worldGrid = gameObject.GetComponent<WorldGrid>();
    }

    private void Start()
    {
        worldGrid.PlaceTileItem(worldGrid.GetXGridSize() / 2 - 1, worldGrid.GetYGridSize() / 2 - 1, Base);
    }

    public void WaveDone(bool allWavesDone)
    {

    }

    public void TowerPlaced()
    {

    }
}
