using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// du kan lave så mange attack varianter som du har loest
public abstract class TowerAbstractAttack
{

    public TowerData towerData;
    public abstract void Attack(Transform shootFrom, Transform ShootTo);

}