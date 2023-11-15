using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{

    public Transform target;
    public float range = 15f;
    [Range(0,100)]
    public float fireRate = 1f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget ()
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

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        Debug.Log("Shoot");

        //Instantiate(bulletPrefab, firePoint.position);
    }
}