using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAbstractAttack
{

    public TowerData towerData;


    //protected virtual void Init()
    //{

    //}

    public abstract void Attack(Transform shootFrom, Transform ShootTo);

}

// du kan lave så mange attack varianter som du har loest

