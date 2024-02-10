using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/EnemyList", fileName = "EnemyList")]
public class EnemyList : ScriptableObject
{
    [field: SerializeField]
    public EnemyData[] EnemyDataSet { get; private set; }

    public GameObject GetEnemy(int ID)
    {
        return EnemyDataSet[ID].Prefab;
    }

    public float GetEnemyDelay(int ID)
    {
        return EnemyDataSet[ID].Delay;
    }
}

[Serializable]
public struct EnemyData 
{
    [field: SerializeField]
    public GameObject Prefab;

    [field: SerializeField]
    public float Delay;

    [field: SerializeField]
    public Image Icon { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }
}