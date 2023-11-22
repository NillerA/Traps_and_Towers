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

    public override void Attack(Transform shootFrom, Transform ShootTo, int damage)
    {
        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<GameObject>("Bullets");
        GameObject bulletGO = (GameObject)GameObject.Instantiate(bulletPrefab, shootFrom.position, shootFrom.rotation);
        BulletScript bullet = bulletGO.GetComponent<BulletScript>();
        bullet.damage = damage;
        bullet.Seek(ShootTo);
    }
}
