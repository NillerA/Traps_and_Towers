using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private int health;
    [SerializeField]
    public GridTileItem Base, forest, monsterCave, swamp;
    [SerializeField]
    TextMeshProUGUI healthText;

    [HideInInspector]
    public GridTile baseTile;
    [SerializeField]
    private Slider healthBar;
    public UnityEvent OnDmageTaken;

    [SerializeField]
    bool topToButtom;

    GameManager() 
    {
        Instance = this;
    }

    private void Awake()
    {
        OnDmageTaken = new UnityEvent();
        if (topToButtom)
        {
            if (GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() / 2 - 1, 0, Base))
            {
                baseTile = GridManager.Instance.GetGridTile(GridManager.Instance.GetXGridSize() / 2 - 1, 0);
                healthBar = GridManager.Instance.GetVisualTile(GridManager.Instance.GetXGridSize() / 2 - 1, 0).transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>();
            }
            else
                Debug.LogError("Could not place base on grid");
            GridManager.Instance.PlaceTileItem(0, GridManager.Instance.GetYGridSize() - 1, monsterCave);
            GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() - 1, monsterCave);
            GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() - 1, GridManager.Instance.GetYGridSize() - 1, monsterCave);
            WaveManager.Instance.SpawnPoints = new List<Point>
            {
                new Point(0,GridManager.Instance.GetYGridSize() - 1),
                new Point(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() - 1),
                new Point(GridManager.Instance.GetXGridSize() - 1,GridManager.Instance.GetYGridSize() - 1)
            };
        }
        else
        {
            if (GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() / 2 - 1, Base))
            {
                baseTile = GridManager.Instance.GetGridTile(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() / 2 - 1);
                healthBar = GridManager.Instance.GetVisualTile(GridManager.Instance.GetXGridSize() / 2 - 1, GridManager.Instance.GetYGridSize() / 2 - 1).transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>();
            }
            else
                Debug.LogError("Could not place base on grid");
            GridManager.Instance.PlaceTileItem(0, 0, monsterCave);
            GridManager.Instance.PlaceTileItem(0, GridManager.Instance.GetYGridSize() - 1, monsterCave);
            GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() - 1, 0, monsterCave);
            GridManager.Instance.PlaceTileItem(GridManager.Instance.GetXGridSize() - 1, GridManager.Instance.GetYGridSize() - 1, monsterCave);
            WaveManager.Instance.SpawnPoints = new List<Point>
            {
                new Point(0,0),
                new Point(0,GridManager.Instance.GetYGridSize() - 1),
                new Point(GridManager.Instance.GetXGridSize() - 1,0),
                new Point(GridManager.Instance.GetXGridSize() - 1,GridManager.Instance.GetYGridSize() - 1)
            };
        }
        for (int i = 0; i < 10;)
        {
            if (GridManager.Instance.PlaceTileItem(Random.Range(0, GridManager.Instance.GetXGridSize() - 1), Random.Range(0, GridManager.Instance.GetYGridSize() - 1), forest))
                i++;
        }
        for (int i = 0; i < 5;)
        {
            if (GridManager.Instance.PlaceTileItem(Random.Range(0, GridManager.Instance.GetXGridSize()), Random.Range(0, GridManager.Instance.GetYGridSize()), swamp))
                i++;
        }
    }

    private void Start()
    {
        healthText.text = "base health: " + health.ToString();
    }

    public void TutorialDone()
    {
        BuildManager.Instance.CanPlaceTurrets();
    }

    bool lost;//midlertidlig
    [SerializeField]
    private GameObject fastWin, fastLose;//midlertidlig
    
    public void WaveDone(bool allWavesDone)
    {
        if (allWavesDone)
        {
            if (!lost)
                fastWin.SetActive(true);
        }
        else
            BuildManager.Instance.CanPlaceTurrets();
    }

    public void TowerPlaced()
    {
        WaveManager.Instance.StartWave();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = "base health: " + health.ToString();
        healthBar.value = health;
        OnDmageTaken.Invoke();
        if (health <= 0)
        {
            fastLose.SetActive(true);
            lost = true;
        }
    }
}
