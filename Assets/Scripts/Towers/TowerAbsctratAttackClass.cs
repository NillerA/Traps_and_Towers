using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAbstractAttack
{

    public TowerData towerData;

    //protected virtual void Init()
    //{

    //}

    public abstract void Attack(Vector3 shootFrom, Vector3 ShootTo);

}

// du kan lave så mange attack varianter som du har loest


public class ArcherTowerAttack : TowerAbstractAttack
{
    public override void Attack(Vector3 shootFrom, Vector3 ShootTo)
    {
        throw new System.NotImplementedException();
    }
}