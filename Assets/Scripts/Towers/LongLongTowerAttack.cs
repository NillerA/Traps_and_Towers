using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongLongTowerAttack : TowerAbstractAttack
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

    public override void Attack(Vector3 shootFrom, Vector3 ShootTo, int damage)
    {
        throw new System.NotImplementedException();
    }
}
