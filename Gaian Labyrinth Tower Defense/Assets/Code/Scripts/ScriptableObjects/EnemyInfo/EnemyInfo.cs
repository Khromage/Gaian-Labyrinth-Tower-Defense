using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBehavior;

[CreateAssetMenu(menuName = "GLTD/EnemyInfo", fileName = "EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    [field: SerializeField]
    public GameObject Prefab;

    //time before spawning this enemy after the previous one
    [field: SerializeField]
    public float Delay;

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }


    [Header("Stats")]
    public float moveSpeed;
    public float maxHealth;
    public int worth;
    public int harm;
    public Weight currentWeight;

    //maybe include damageIndicator and healthbar? might be more efficient to put those in here than in their behavior scripts?
}
