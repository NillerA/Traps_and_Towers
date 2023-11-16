using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private EnemyStats goblinStats;

    int enemyAmount;
    [SerializeField]
    private GameObject enemyPrefab;
    public Queue<GameObject> enemyPool=new Queue<GameObject>();
    private List<GameObject> activeEnemies=new List<GameObject>();

    [SerializeField]
    private List<WaveStats> Waves=new List<WaveStats>();

    


    public bool startNextWave = false;

    private int currentWave=0;

    void Start()
    {
        enemyAmount = 5;

        for (int i = 0; i < 5; i++)
        {
           
            GameObject newEnemy = Instantiate(enemyPrefab);
            newEnemy.SetActive(false);
            newEnemy.transform.position = new Vector3(0, 0, 0);
              
            enemyPool.Enqueue(newEnemy);
        }


        startNextWave = true;

    }

    // Update is called once per frame
    void Update()
    {

        //if (activeEnemies.Count>0)
        //{
        //    foreach (var enemy in activeEnemies)
        //    {
        //        if (enemy.GetComponent<EnemyMovement>().currentHealth <= 0)
        //        {
        //            ReleaseEnemy(enemy);
        //        }
        //    }
        //}


        if(startNextWave==true)
        {
            StartWave();

            startNextWave = false;
        }

    }


    public void StartWave()
    {
     

        for (int i = 0; i < Waves[currentWave].goblinAmount; i++)
        {
            GameObject e = GetEnemy();

           
            e.GetComponent<EnemyMovement>().stats = goblinStats;

            e.GetComponent<EnemyMovement>().setUpEnemy(AStar());

            activeEnemies.Add(e);

        }

        foreach (var enemy in activeEnemies)
        {

            enemy.SetActive(true);

        }

        currentWave += 1;

    }
    public List<Vector3> AStar()
    {
        List<Vector3> path = new List<Vector3>();

        path.Add(new Vector3(-10, 0,1));

        path.Add(new Vector3(10, 0,2));

        path.Add(new Vector3(-60,0, 5));


        return path;
    }

    public GameObject CreateGoblin()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);
        newEnemy.transform.position = new Vector3(0, 0, 0);
        enemyPool.Enqueue(newEnemy);

        return newEnemy;
    }


    public GameObject GetEnemy()
    {
        GameObject enemy;
        if(enemyPool.Count==0)
        {
            enemy=CreateGoblin();
        }
        else
        {
            enemy = enemyPool.Dequeue();
        }

        return enemy;
    }

    public void ReleaseEnemy( GameObject deadEnemy)
    {

        deadEnemy.SetActive(false);
        CleanUp(deadEnemy);
        enemyPool.Enqueue(deadEnemy);
        activeEnemies.Remove(deadEnemy);

      
        
    }

    public void CleanUp(GameObject deadenemy)
    {
        EnemyMovement eM =deadenemy.GetComponent<EnemyMovement>();

        eM.currentHealth = eM.maxHealth;
        

    }
    
}
