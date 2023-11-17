using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{

    public Transform target;

    public TowerAbstractAttack towerAttack = new ArcherTowerAttack();
    //private float fireCountdown = 0f;
    public TowerData towerData;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        StartCoroutine(ShootLoop());
    }

    bool UpdateTarget()
    {
        if (WaveManager.Instance.activeEnemies.Count == 0)
        return false;
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in WaveManager.Instance.activeEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= towerData.viewDistance)
        {
            target = nearestEnemy.transform;
            return true;
        }
        else
        {
            target = null;
            return false;
        }
    }

    //void Update()
    //{
    //    if (target == null)
    //    {
    //        return;
    //    }
    //    if (Vector3.Distance(transform.position, target.transform.position) < towerData.viewDistance)
    //    {
    //        if (fireCountdown <= 0f)
    //        {
    //            towerAttack.Attack(transform, target);
    //            fireCountdown = 1f / towerData.attackSpeed;
    //        }
    //        fireCountdown -= Time.deltaTime;
    //    }
    //}

    private IEnumerator ShootLoop() 
    {
        while (true)
        {
            yield return new WaitForSeconds(towerData.attackSpeed);
            if (UpdateTarget())
            {
                towerAttack.Attack(transform, target);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(towerData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, towerData.viewDistance);
        }
    }
}
