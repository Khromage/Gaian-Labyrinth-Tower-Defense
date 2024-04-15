using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    
    private float damagePerTick;

    [SerializeField]
    private float tickRate = 2f;

    private int remainingTicks;

    public Burn(float duration, float damagePerTick)
    {
        this.duration = duration;
        this.damagePerTick = damagePerTick;
        remainingTicks = (int)(duration * tickRate);
        Debug.Log($"New Burn's total remaining ticks: {remainingTicks}");
        id = DebuffList.burnID;
    }
    public Burn(float duration, float damagePerTick, float tickRate)
    {
        this.duration = duration;
        this.damagePerTick = damagePerTick;
        this.tickRate = tickRate;
        remainingTicks = (int)(duration * tickRate);
        id = DebuffList.burnID;
    }

    public override void Effect(GameObject enemy, List<StatusEffect> vulnList)
    {
        if (enemy.tag == "Enemy")
        {
            EnemyBehavior e = enemy.GetComponent<EnemyBehavior>();
            if (remainingTicks > 0 && timeElapsed > duration - (remainingTicks / tickRate))
            {
                if (e.enemyWeight == EnemyBehavior.Weight.light)
                {
                    //weight stuff idk
                }
                Debug.Log("Dealing a tick of burn damage!");
                e.takeDamage(damagePerTick);
                remainingTicks--;
            }
        }
        else
        {
            Debug.Log("Trying to burn a non-enemy");
        }
    }
}
