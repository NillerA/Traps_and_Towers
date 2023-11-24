using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTower : TowerScript
{

    private List<Vector3> GridTiles;
    
    public override void Start()
    {
        towerAttack = new MeteorAttack();
        (int x, int y) = GridManager.Instance.WorldToGrid(transform.position);
        GridTiles = GridManager.Instance.GetTilesInRadius(new Point(x,y), 1);
        base.Start();
    }

    public override IEnumerator ShootLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(towerData.attackSpeed);
            RandomTilePlacement();
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
}
