using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private int health;
    [SerializeField]
    private WorldGrid worldGrid;
    [SerializeField]
    private GridTileItem Base, forest;
    [SerializeField]
    private Shop towerPlacementManager;
    //add wave manager

    public GridTile baseTile;

    GameManager() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Awake()
    {
        if (worldGrid == null)
            worldGrid = gameObject.GetComponent<WorldGrid>();
    }

    private void Start()
    {
        if (worldGrid.PlaceTileItem(worldGrid.GetXGridSize() / 2 - 1, worldGrid.GetYGridSize() / 2 - 1, Base))
            baseTile = WorldGrid.Instance.GetGridTile(worldGrid.GetXGridSize() / 2 - 1, worldGrid.GetYGridSize() / 2 - 1);
        else
            Debug.LogWarning("Could not place base on grid");
        for (int i = 0; i < 5;)
        {
            if (worldGrid.PlaceTileItem(Random.Range(1, worldGrid.GetXGridSize() - 1), Random.Range(1, worldGrid.GetYGridSize() - 1), forest))
                i++;
        }
        towerPlacementManager.CanPlaceTurret();
    }

    public void WaveDone(bool allWavesDone)
    {
        if (allWavesDone)
            Debug.Log("U WIN");
        else
            towerPlacementManager.CanPlaceTurret();
    }

    public void TowerPlaced()
    {
        //start next wave
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health! > 0)
            Debug.Log("U LOSE");
    }
}
