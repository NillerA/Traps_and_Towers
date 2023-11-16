using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Wave", menuName = "Game/WaveStats", order = 2)]
public class WaveStats : ScriptableObject
{

    [SerializeField]
    public List<EnemySpawnInfo> waveStatsList;

}

[Serializable]
public struct EnemySpawnInfo
{
    [Tooltip("The enemy to spawn")]
    public EnemyStats enemyType;
    [Tooltip("The amount to spawn")]
    public int spawnAmount;
    [Tooltip("Spawnrate in seconds")]
    public float SpawnRate;
}