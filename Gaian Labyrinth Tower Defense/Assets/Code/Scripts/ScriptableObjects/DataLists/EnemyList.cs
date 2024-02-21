using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/EnemyList", fileName = "EnemyList")]
public class EnemyList : ScriptableObject
{
    [field: SerializeField]
    public EnemyInfo[] EnemyDataSet { get; private set; }

    public GameObject GetEnemy(int ID)
    {
        return EnemyDataSet[ID].Prefab;
    }

    public float GetEnemyDelay(int ID)
    {
        return EnemyDataSet[ID].Delay;
    }
}