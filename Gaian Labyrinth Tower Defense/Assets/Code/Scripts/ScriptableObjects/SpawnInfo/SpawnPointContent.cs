using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnPointContent : ScriptableObject
{
    //public SpawnPointStruct[] spawnPoints;
    public WaveStruct[] waves;
}

[System.Serializable]
public struct WaveStruct
{
    public GameObject[] waveEnemies;
}

[System.Serializable]
public struct SpawnPointStruct
{
    public WaveStruct[] pointContent;
}