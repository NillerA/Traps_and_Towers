using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "TowerData", menuName = "Game/Items/TowerData", order = 1)]
public class TowerData : ScriptableObject
{

    [Range(0, 100)]
    public int damage, attackSpeed, viewDistance;


    public GameObject model;

}
