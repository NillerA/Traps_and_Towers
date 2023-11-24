using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    public EnemyStats stats;
    public string enemyName;
    public int currentHealth;

    public int damage;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private GameObject healthBarSeperator;
    private List<GameObject> healthBarSeperators;

    int currentPathTarget = 0;
    [SerializeField]
    private List<Vector3> path = new List<Vector3>();
    private Vector3 direction;
    GameObject enemyVisual;

    void Update()
    {
        if (currentPathTarget < path.Count)
        {
            direction = new Vector3(path[currentPathTarget].x - gameObject.transform.position.x,0, path[currentPathTarget].z - gameObject.transform.position.z);

            if (direction.magnitude <= 0.1f)
                currentPathTarget += 1;
            else
            {
                direction.Normalize();
                (int x, int y) currentTile = GridManager.Instance.WorldToGrid(transform.position);
                float tileSpeed = 1;
                if(GridManager.Instance.GetGridTile(currentTile.x, currentTile.y).GridTileItem != null)
                    tileSpeed = GridManager.Instance.GetGridTile(currentTile.x, currentTile.y).GridTileItem.WalkSpeed;
                gameObject.transform.position += new Vector3((direction.x * stats.speed) * tileSpeed * Time.deltaTime, 0, (direction.z * stats.speed) * tileSpeed * Time.deltaTime);
            }
        }
        else
            WaveManager.Instance.ReachedBase(gameObject);
    }

    public bool TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            WaveManager.Instance.ReleaseEnemy(gameObject);
            return true;
        }
        return false;
    }

    public void setUpEnemy(List<Vector3> newPath, EnemyStats newStats)
    {
        currentPathTarget = 0;
        path = newPath;
        transform.position = newPath[0];

        stats = newStats;
        enemyName = stats.name;
        currentHealth = stats.maxHealth;
        healthSlider.maxValue = stats.maxHealth;
        healthSlider.value = stats.maxHealth;
        if (healthBarSeperators != null)
            for (int i = 0; i < healthBarSeperators.Count; i++)
                Destroy(healthBarSeperators[i]);
        healthBarSeperators = new List<GameObject>();
        for (int i = 1; i < stats.maxHealth; i++) 
            healthBarSeperators.Add(Instantiate(healthBarSeperator, healthBarSeperator.transform.parent));

        if (enemyVisual != null)
            Destroy(enemyVisual);
        enemyVisual = Instantiate(stats.enemyvisualPrefab, transform.position, transform.rotation, transform);
    }
}
