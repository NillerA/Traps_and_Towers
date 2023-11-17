using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{

    public Transform target;
    public float range = 15f;
    public bool shootIsTrue;

    public TowerAbstractAttack towerAttack = new ArcherTowerAttack();
    //bulletprefab er en objekt jeg gører brug af fordi jeg ikke har bullets


    //public TowerScript tower;
    private float fireCountdown = 0f;

    public TowerData towerData;/* = new TowerData();*/


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        //float shortestDistance = Mathf.Infinity;
        //Wavemanager.Enemy nearestEnemy = null;
        //foreach (WaveManager enemy in enemies)
        //{
        //    float distanceToEnemy = Vector3.Distance(transform.position, enemy.tranform.posiiton);

        //    if (distanceToEnemy < shortestDistance)
        //    {
        //        shortestDistance = distanceToEnemy;
        //        nearestEnemy = enemy;
        //    }
        //}
        //if (nearestEnemy != null && shortestDistance <= range)
        //{
        //    target = nearestEnemy.transform;
        //}
        //else
        //{
        //    target = null;
        //}
    }
    // Update is called once per frame
    void Update()
    {



        if (target == null)
        {
            return;
        }

        //if (range <= 15f)
        //{
        //    shootIsTrue = false;
        //}
        //else
        //{
        //    shootIsTrue = true;
        //}

        if (Vector3.Distance(transform.position, target.transform.position) < towerData.viewDistance)
        {
            if (fireCountdown <= 0f)
            {
                towerAttack.Attack(transform, target);
                fireCountdown = 1f / towerData.attackSpeed;
            }
            fireCountdown -= Time.deltaTime;
        }


    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
