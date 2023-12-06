using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTower : TowerScript
{

    private List<Vector3> GridTiles;

    public void Awake()
    {
        CheckNeighbours();
    }

    public override void Start()
    {
        towerAttack = new MeteorAttack();
        BuildManager.Instance.OnTowerPlaced.AddListener(CheckNeighbours);
        base.Start();
    }

    public override IEnumerator ShootLoop()
    {
        while (true)
        {
            if(RandomTilePlacement())
                yield return new WaitForSeconds(towerData.attackSpeed);
            else
                yield return null;
        }
    }

    public bool RandomTilePlacement()
    {
        if (WaveManager.Instance.activeEnemies.Count == 0)
            return false;
        else
        {
            int tileToAttck = Random.Range(0, GridTiles.Count);

            towerAttack.Attack(GridTiles[tileToAttck] + Vector3.up * 10, GridTiles[tileToAttck], towerData.damage);
            return true;
        }
    }

    public void CheckNeighbours()
    {
        (int x, int y) = GridManager.Instance.WorldToGrid(transform.position);
        GridTiles = GridManager.Instance.GetTilesInRadius(new Point(x, y), 1);
        for (int i = 0; i < GridTiles.Count; i++)
        {
            Vector3 tile = GridTiles[i];
            (x, y) = GridManager.Instance.WorldToGrid(tile);
            if (!GridManager.Instance.IsWalkable(new Point(x, y)))
            {
                GridTiles.Remove(tile);
                i--;
            }
        }
    }
}
