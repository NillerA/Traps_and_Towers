using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private int health;
    [SerializeField]
    private GridTileItem Base, forest, monsterCave;
    [SerializeField]
    private Shop towerPlacementManager;

    public GridTile baseTile;

    GameManager() 
    {
        Instance = this;
    }

    private void Start()
    {
        if (WorldGrid.Instance.PlaceTileItem(WorldGrid.Instance.GetXGridSize() / 2 - 1, WorldGrid.Instance.GetYGridSize() / 2 - 1, Base))
            baseTile = WorldGrid.Instance.GetGridTile(WorldGrid.Instance.GetXGridSize() / 2 - 1, WorldGrid.Instance.GetYGridSize() / 2 - 1);
        else
            Debug.LogWarning("Could not place base on grid");
        WorldGrid.Instance.PlaceTileItem(0, 0, monsterCave);
        WorldGrid.Instance.PlaceTileItem(0, WorldGrid.Instance.GetYGridSize() - 1, monsterCave);
        WorldGrid.Instance.PlaceTileItem(WorldGrid.Instance.GetXGridSize() - 1, 0, monsterCave);
        WorldGrid.Instance.PlaceTileItem(WorldGrid.Instance.GetXGridSize() - 1, WorldGrid.Instance.GetYGridSize() - 1, monsterCave);
        for (int i = 0; i < 5;)
        {
            if (WorldGrid.Instance.PlaceTileItem(Random.Range(1, WorldGrid.Instance.GetXGridSize() - 1), Random.Range(1, WorldGrid.Instance.GetYGridSize() - 1), forest))
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
        WaveManager.Instance.StartWave();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Debug.Log("U LOSE");
    }
}
