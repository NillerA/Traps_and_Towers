using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelfDestructScript : MonoBehaviour
{

    [SerializeField]
    private int LifespanInWaves;

    private int WaveSpawnedAt;

    private Point towerPoint;

    void Start()
    {
        
        WaveSpawnedAt = WaveManager.Instance.currentWave;

        (int x, int y) point = GridManager.Instance.WorldToGrid(transform.position);
        towerPoint =new Point(point.x,point.y);


    }

     void Update()
    {
        if(WaveManager.Instance.currentWave>=WaveSpawnedAt+LifespanInWaves)
        {
            
            List<Point> neighbors=GridManager.Instance.GetGridTileNeiboursPoints(towerPoint);

            
            foreach(var item in neighbors)
            {

                GridManager.Instance.RemoveTileItem(item.X, item.Y);
            }

            GridManager.Instance.RemoveTileItem(towerPoint.X,towerPoint.Y);
        }
    }



   
}
