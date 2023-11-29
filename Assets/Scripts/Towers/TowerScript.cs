using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerScript : MonoBehaviour
{

    public Transform target;

    public TowerAbstractAttack towerAttack = new ArcherTowerAttack();
    //private float fireCountdown = 0f;
    public TowerData towerData;

    [SerializeField]
    private CircleRend shootRadiusDisplay;

    private bool towerTaunted=false;
    public virtual void Start()
    {
        StartCoroutine(ShootLoop());
    }

    private bool UpdateTarget()
    {
        if (WaveManager.Instance.activeEnemies.Count == 0)
        return false;
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        towerTaunted = false;
        foreach (GameObject enemy in WaveManager.Instance.activeEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance && towerTaunted==false)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;

                if(enemy.GetComponent<EnemyMovement>().stats.Taunt==true)
                {
                    towerTaunted = true;
                }
            }

            //if (distanceToEnemy <= towerData.viewDistance && enemy.GetComponent<EnemyStats>().Taunt == true)
            //{
            //    shortestDistance = distanceToEnemy;
            //    nearestEnemy = enemy;
            //    towerTaunted = true;
            //}

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

    public virtual IEnumerator ShootLoop() 
    {
        while (true)
        {
            yield return new WaitForSeconds(towerData.attackSpeed);
            if (UpdateTarget())
            {
                towerAttack.Attack(transform, target, towerData.damage);
            }
        }
    }


    private void OnMouseEnter()
    {
        shootRadiusDisplay.Draw(towerData.viewDistance);
        BuildManager.Instance.ShowStatsDisplay(towerData);
    }

    private void OnMouseExit()
    {
        shootRadiusDisplay.Hide();
        BuildManager.Instance.HideStatsDisplay();
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
