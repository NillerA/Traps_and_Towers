using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorAttack : TowerAbstractAttack
{
    public GameObject meteorPrefab;


    public void Setup()
    {
       meteorPrefab = Resources.Load<GameObject>("Meteor");
    }

    public override void Attack(Transform shootFrom, Transform ShootTo, int damage)
    {
        throw new System.NotImplementedException();
        
    }

    public override void Attack(Vector3 shootFrom, Vector3 ShootTo, int damage)
    {
        if (meteorPrefab == null)
            meteorPrefab = Resources.Load<GameObject>("Meteor");
        GameObject bulletGO = GameObject.Instantiate(meteorPrefab, shootFrom, Quaternion.identity);
        Meteors bullet = bulletGO.GetComponent<Meteors>();
        bullet.damage = damage;
    }
}
