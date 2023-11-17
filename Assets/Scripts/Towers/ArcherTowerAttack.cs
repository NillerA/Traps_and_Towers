using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ArcherTowerAttack : TowerAbstractAttack
{
    public GameObject bulletPrefab;

    public void Setup()
    {
        bulletPrefab = Resources.Load<GameObject>("Bullets");
    }

    public override void Attack(Transform shootFrom, Transform ShootTo)
    {

        if (bulletPrefab == null)
        {
            //Debug.LogWarning("prefab missing finding prefab");
            bulletPrefab = Resources.Load<GameObject>("Bullets");
            //if(bulletPrefab == null)
            //{
            //    Debug.LogError("Bullet prefab not found");
            //}
        }

        Debug.Log("Shoot");

        GameObject bulletGO = (GameObject)GameObject.Instantiate(bulletPrefab, shootFrom.position, shootFrom.rotation);
        BulletScript bullet = bulletGO.GetComponent<BulletScript>();

        if (bullet != null)
        {
            bullet.Seek(ShootTo);
        }

    }
}
