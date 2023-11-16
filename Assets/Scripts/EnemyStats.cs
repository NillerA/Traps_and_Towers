using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Enemy",menuName ="Game/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    
    public int maxHealth;
    public int damage;

    public GameObject enemyvisualPrefab;

}
