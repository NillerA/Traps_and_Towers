using System.Collections;
using System.Collections.Generic;
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
    public int currentWave = 0;
    private GameObject enemyHolder;
    private List<Vector3> currentPath;

    public List<GameObject> activeEnemies = new List<GameObject>();
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    bool waveSpawningDone = false;

    [SerializeField]
    private LineRenderer pathLineRend;

    public List<Point> SpawnPoints;
    int currentSpawn;

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
        currentSpawn = Random.Range(0,SpawnPoints.Count);
        UpdatePath();
        wavesLeftText.text = "Waves left: " + (Waves.Count - currentWave);
    }

    public void StartWave()
    {
        UpdatePath();
        waveSpawningDone = false;
        StartCoroutine(Wave());
    }

    private IEnumerator Wave()
    {
        float waveSpawnTimer = 0;
        List<float> spawnTimes = new List<float>();
        List<int> spawnedEnemies = new List<int>();
        int enemysToSpawn = 0;
        int spawnedEnemiesAmount = 0;
        for (int i = 0; i < Waves[currentWave].waveStatsList.Count; i++)
        {
            spawnTimes.Add(0);
            spawnedEnemies.Add(0);
            enemysToSpawn += Waves[currentWave].waveStatsList[i].spawnAmount;
        }
        while (!waveSpawningDone)
        {
            waveSpawnTimer += Time.deltaTime;
            for (int i = 0; i < Waves[currentWave].waveStatsList.Count; i++)
            {
                if (spawnedEnemies[i] < Waves[currentWave].waveStatsList[i].spawnAmount && spawnTimes[i] < waveSpawnTimer)
                {
                    spawnTimes[i] += Waves[currentWave].waveStatsList[i].SpawnRate;
                    spawnedEnemies[i]++;
                    spawnedEnemiesAmount++;
                    GameObject spawnedEnemy = GetEnemy();
                    spawnedEnemy.GetComponent<EnemyMovement>().setUpEnemy(currentPath, Waves[currentWave].waveStatsList[i].enemyType);
                    activeEnemies.Add(spawnedEnemy);
                    spawnedEnemy.SetActive(true);
                    spawnedEnemy.GetComponent<EnemyMovement>().StartEnemy();
                }
            }
            if(enemysToSpawn <= spawnedEnemiesAmount)
                waveSpawningDone = true;
            yield return null;
        }
        //for (int x = 0; x < Waves[currentWave].waveStatsList.Count; x++)
        //{
        //    for (int i = 0; i < Waves[currentWave].waveStatsList[x].spawnAmount; i++)
        //    {
        //        yield return new WaitForSeconds(Waves[currentWave].waveStatsList[x].SpawnRate);

        //        //TODO: add a way to spawn the different enemys at once (EnemySpawnInfo)
        //        GameObject spawnedEnemy = GetEnemy();
        //        spawnedEnemy.GetComponent<EnemyMovement>().setUpEnemy(currentPath, Waves[currentWave].waveStatsList[x].enemyType);
        //        activeEnemies.Add(spawnedEnemy);
        //        spawnedEnemy.SetActive(true);
        //    }
        //}
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
            currentSpawn = Random.Range(0, SpawnPoints.Count);
            UpdatePath();
        }
    }

    public void UpdatePath()
    {
        currentPath = aStar.GetPath(SpawnPoints[currentSpawn], new Point(GameManager.Instance.baseTile.AStarInfo.xCord, GameManager.Instance.baseTile.AStarInfo.yCord));
        pathLineRend.positionCount = currentPath.Count;
        for (int i = 0; i < currentPath.Count; i++)
            pathLineRend.SetPosition(i, currentPath[i] + new Vector3(0,0.15f,0));
    }

    public bool UpdatePath(Point tileToIgnore)
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (i == currentSpawn)
                continue;
            List<Vector3> testPath = aStar.GetPath(SpawnPoints[i], new Point(GameManager.Instance.baseTile.AStarInfo.xCord, GameManager.Instance.baseTile.AStarInfo.yCord), tileToIgnore);
            if (testPath == null)
                return false;
        }
        currentPath = aStar.GetPath(SpawnPoints[currentSpawn], new Point(GameManager.Instance.baseTile.AStarInfo.xCord, GameManager.Instance.baseTile.AStarInfo.yCord), tileToIgnore);
        if (currentPath == null)
            return false;
        pathLineRend.positionCount = currentPath.Count;
        for (int i = 0; i < currentPath.Count; i++)
            pathLineRend.SetPosition(i, currentPath[i] + new Vector3(0, 0.15f, 0));
        return true;
    }
}