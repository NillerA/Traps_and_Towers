using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager Instance;

    [SerializeField]
    private AStarBackup aStar;
    [SerializeField]
    private EnemyStats goblinStats;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<WaveStats> Waves = new List<WaveStats>();
    [SerializeField]
    private TextMeshProUGUI wavesLeftText;
    private int currentWave = 0;
    private GameObject enemyHolder;
    private List<Vector3> currentPath;

    public List<GameObject> activeEnemies = new List<GameObject>();
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    bool waveSpawningDone = false;

    WaveManager() 
    {
        Instance = this;
    }

    private void Awake()
    {
        activeEnemies = new List<GameObject>();
    }

    void Start()
    {
        enemyHolder = new GameObject("Enemys");
        enemyHolder.transform.parent = transform;
        enemyHolder.transform.position = Vector3.zero;
        for (int i = 0; i < 25; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, enemyHolder.transform);
            newEnemy.SetActive(false);
            enemyPool.Enqueue(newEnemy);
        }
    }

    public void StartWave()
    {
        waveSpawningDone = false;
        currentPath = aStar.GetPath(new Point(0, 0), new Point(4, 4));
        StartCoroutine(Wave());
    }

    private IEnumerator Wave()
    {
        for (int i = 0; i < Waves[currentWave].waveStatsList[0].spawnAmount; i++)
        {
            yield return new WaitForSeconds(Waves[currentWave].waveStatsList[0].SpawnRate);

            //TODO: add a way to spawn the different enemys at once (EnemySpawnInfo)
            GameObject spawnedEnemy = GetEnemy();
            spawnedEnemy.GetComponent<EnemyMovement>().setUpEnemy(currentPath, Waves[currentWave].waveStatsList[0].enemyType);
            activeEnemies.Add(spawnedEnemy);
            spawnedEnemy.SetActive(true);
        }
        waveSpawningDone = true;
    }

    public List<Vector3> AStar()
    {
        List<Vector3> path = new List<Vector3>
        {
            new Vector3(0,0,0),
            new Vector3(10, 0, 0),
            new Vector3(10, 0, 7.5f),
            new Vector3(5, 0, 7.5f),
            new Vector3(5, 0, 3.75f)
        };

        return path;
    }

    public GameObject CreateNewEnemy()
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
            enemy=CreateNewEnemy();
        else
            enemy = enemyPool.Dequeue();

        return enemy;
    }

    public void ReachedBase(GameObject enemyToReachEnd)
    {
        GameManager.Instance.TakeDamage(enemyToReachEnd.GetComponent<EnemyMovement>().stats.damage);
        ReleaseEnemy(enemyToReachEnd);
    }

    public void ReleaseEnemy(GameObject enemyToRelease)
    {
        enemyToRelease.SetActive(false);
        activeEnemies.Remove(enemyToRelease);
        enemyPool.Enqueue(enemyToRelease);
        if (activeEnemies.Count == 0 && waveSpawningDone)
        {
            currentWave += 1;
            GameManager.Instance.WaveDone(currentWave >= Waves.Count);
            wavesLeftText.text = "Waves left: " + (Waves.Count - currentWave);
        }
    }
}
