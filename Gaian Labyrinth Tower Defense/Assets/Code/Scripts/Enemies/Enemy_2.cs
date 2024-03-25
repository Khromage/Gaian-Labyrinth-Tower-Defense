using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : EnemyBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 50f;
        currentHealth = maxHealth;
        moveSpeed = 2f;
        harm = 1;
        worth = 20;
        EnemyHurtSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
}
