using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : ScriptableObject
{
    //time before spawning this enemy after the previous one
    public int ID;
    public float spawnDelay;

    public float moveSpeed;
    public float maxHealth;
    public int worth;
    public int harm;

    //maybe include damageIndicator and healthbar? might be more efficient to put those in here than in their behavior scripts?
}
