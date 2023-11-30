using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "TowerData", menuName = "Game/Items/TowerData", order = 1)]
public class TowerData : ScriptableObject
{

    [Range(0, 10)]
    public int damage;
    [Range(0,10)]
    public float attackSpeed,viewDistance;
    public string attackType = "Single";
    public string description = "description of Tower";

}
