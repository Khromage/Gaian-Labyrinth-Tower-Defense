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
    }
    public Burn(float duration, float damagePerTick, float tickRate)
    {
        this.duration = duration;
        this.damagePerTick = damagePerTick;
        this.tickRate = tickRate;
        remainingTicks = (int)(duration * tickRate);
    }

    public override void Effect(EnemyBehavior subject)
    {
        if (remainingTicks > 0 && timeElapsed > duration - remainingTicks * tickRate)
        {
            subject.takeDamage(damagePerTick);
            remainingTicks--;
        }
    }
}
