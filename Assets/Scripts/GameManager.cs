using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private int health;
    [SerializeField]
    private GridTileItem Base, forest, monsterCave;
    [SerializeField]
    private Shop towerPlacementManager;
    [SerializeField]
    TextMeshProUGUI healthText;

    public GridTile baseTile;
    public UnityEvent OnDmageTaken;

    GameManager() 
    {
        Instance = this;
    }

    private void Awake()
    {
        OnDmageTaken = new UnityEvent();
        if (GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() / 2 - 1, Base))
            baseTile = GridManager.Instance.GetGridTile(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() / 2 - 1);
        else
            Debug.LogWarning("Could not place base on grid");
        GridManager.Instance.PlaceTileItem(0, 0, monsterCave);
        GridManager.Instance.PlaceTileItem(0, GridManager.Instance.GetYGridSize() - 1, monsterCave);
        GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() - 1, 0, monsterCave);
        GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() - 1, GridManager.Instance.GetYGridSize() - 1, monsterCave);
        for (int i = 0; i < 5;)
        {
            if (GridManager.Instance.PlaceTileItem(Random.Range(1, GridManager.Instance.GetXGridSize() - 1), Random.Range(1, GridManager.Instance.GetYGridSize() - 1), forest))
                i++;
        }
    }

    private void Start()
    {
        towerPlacementManager.CanPlaceTurret();
    }

    bool lost;//midlertidlig
    [SerializeField]
    private GameObject fastWin, fastLose;//midlertidlig
    
    public void WaveDone(bool allWavesDone)
    {
        if (allWavesDone)
        {
            if(!lost)
                fastWin.SetActive(true);
        }
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
        healthText.text = health.ToString();
        OnDmageTaken.Invoke();
        if (health <= 0)
        {
            fastLose.SetActive(true);
            lost = true;
        }
    }
}
