using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTower : MonoBehaviour
{

    public TowerAbstractAttack towerAttack = new MeteorAttack();

    public TowerData towerData;
    private List<Vector3> GridTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        (int x, int y) = GridManager.Instance.WorldToGrid(transform.position);
        GridTiles = GridManager.Instance.GetTilesInRadius(new Point(x,y), 1);
    }

    public bool RandomTilePlacement()
    {
        if (WaveManager.Instance.activeEnemies.Count == 0)
            return false;



        if (WaveManager.Instance.activeEnemies.Count == 1)
        {
            int tileToAttck = Random.Range(0, GridTiles.Count);

            towerAttack.Attack(transform.position, GridTiles[tileToAttck], towerData.damage);
            return true;
        }
            

        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
