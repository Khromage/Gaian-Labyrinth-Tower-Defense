using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScout : EnemyBehavior
{


    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        //half as tanky as normal unit
        maxHealth = 6f;
        currentHealth = maxHealth;
        moveSpeed = 6f;
        harm = 1;
        worth = 5;
        EnemyHurtSFX = GetComponent<AudioSource>();
    }


}
