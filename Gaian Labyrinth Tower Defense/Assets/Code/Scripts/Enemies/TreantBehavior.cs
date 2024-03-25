using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantBehavior : EnemyBehavior
{
    private float healthRegenRate;
    private float healthTimer;
    private float healthRegenPercent;
    private float healthTimerDelay;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //Put these into Treant's scriptable object
        healthTimer = 0f;
        healthTimerDelay = 2f;
        healthRegenPercent = 5f;
        
        healthRegenRate = (float)Math.Round(maxHealth * healthRegenPercent / 100, MidpointRounding.AwayFromZero);
        if (healthRegenRate == 0)
        {
            healthRegenRate = 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentHealth != maxHealth){
            regenHealth();
        }
    }

    private void regenHealth()
    {
        if (healthTimer <= healthTimerDelay)
        {
            healthTimer += Time.deltaTime;
        }
        else
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += healthRegenRate;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }
            float spd = 5 + 5 * healthRegenRate / maxHealth;
            HealthBar.SetHealth(currentHealth / maxHealth, spd);

            healthTimer = 0f;
        }
    }
}
