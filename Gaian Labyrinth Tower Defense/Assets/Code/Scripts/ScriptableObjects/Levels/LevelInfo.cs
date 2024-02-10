using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/LevelInfo", fileName = "LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }
        
    [field: SerializeField]
    public WaveInfo[] Waves { get; private set; }
}

[Serializable]
public struct WaveInfo
{    
    [field: SerializeField]
    public SpawnPointInfo[] SpawnPoints { get; private set; }

    [field: SerializeField]
    public TimeSpan Duration { get; private set; }
}

[Serializable]
public struct SpawnPointInfo
{
    [field: SerializeField]
    public int[] SpawnSet; // List of EnemyIDs to spawn at this spawn point
}