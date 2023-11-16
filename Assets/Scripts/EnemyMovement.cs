using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyStats stats;
    public string enemyName;
    public int maxHealth;
    public int currentHealth;

    public int damage;
    float speed = 5;

    int currentStep=0;
    // Start is called before the first frame update
    [SerializeField]
    private List<Vector3> path = new List<Vector3>();
    private Vector3 velocity;
    GameObject enemyVisual;

   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       

        velocity = new Vector3(path[currentStep].x - gameObject.transform.position.x,0, path[currentStep].z - gameObject.transform.position.z);

        Debug.Log($"{velocity.x},{velocity.y},{velocity.z}");
        Debug.Log($"{path.Count}");
      
        if (currentStep+1 != path.Count && velocity.magnitude <= 1)
        {
            currentStep += 1;
        }

        if (velocity != Vector3.zero && velocity.magnitude >1)
        {
            velocity.Normalize();

            gameObject.transform.position+=new Vector3((velocity.x * speed) * Time.deltaTime,0, (velocity.z *speed)*Time.deltaTime);

        }

    }

    public void setUpEnemy(List<Vector3> path)
    {
        enemyName = stats.enemyName;
        maxHealth = stats.maxHealth;

        this.path = path;
       

        if (enemyVisual != null)
        {
            Destroy(enemyVisual);
        }
        enemyVisual = Instantiate(stats.enemyvisualPrefab, transform.position, transform.rotation, transform);


    }

}
