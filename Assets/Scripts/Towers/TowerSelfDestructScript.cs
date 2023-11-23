using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelfDestructScript : MonoBehaviour
{

    [SerializeField]
    private int LifespanInWaves;

    private int WaveSpawnedAt;



    
    void Start()
    {
        
        WaveSpawnedAt = WaveManager.Instance.currentWave;

    }

     void Update()
    {
        if(WaveManager.Instance.currentWave>=WaveSpawnedAt+LifespanInWaves)
        {
            Destroy(gameObject);
        }
       
    }



   
}
