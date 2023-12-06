using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerScript : MonoBehaviour
{

    public Transform target;
    public TowerAbstractAttack towerAttack = new ArcherTowerAttack();
    public TowerData towerData;

    [SerializeField]
    private CircleRend shootRadiusDisplay;
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
        bool towerTaunted = false;

        foreach (GameObject enemy in WaveManager.Instance.activeEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < towerData.viewDistance)
            {
                if (distanceToEnemy < shortestDistance)
                {
                    if(enemy.GetComponent<EnemyMovement>().stats.Taunt == true)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                        towerTaunted = true;
                    }
                    else if (towerTaunted == false)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
                else if(enemy.GetComponent<EnemyMovement>().stats.Taunt == true && towerTaunted == false)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                    towerTaunted = true;
                }
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

    public virtual IEnumerator ShootLoop() 
    {
        while (true)
        {
            if (UpdateTarget())
            {
                towerAttack.Attack(transform, target, towerData.damage);
                yield return new WaitForSeconds(towerData.attackSpeed);
            }
            else
                yield return null;
        }
    }


    private void OnMouseEnter()
    {
        shootRadiusDisplay.Draw(towerData.viewDistance);
        BuildManager.Instance.ShowTowerStatsDisplay(towerData);
    }

    private void OnMouseExit()
    {
        shootRadiusDisplay.Hide();
        BuildManager.Instance.HideStatsDisplay();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(towerData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, towerData.viewDistance);
        }
    }
#endif
}
