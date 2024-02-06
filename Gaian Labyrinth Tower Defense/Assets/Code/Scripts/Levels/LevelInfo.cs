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
    public int AssignedSpawnPoint { get; private set; }
    
    [field: SerializeField]
    public int[] SpawnSet { get; private set; } // list of EnemyIDs

    [field: SerializeField]
    public TimeSpan Duration { get; private set; }
}
