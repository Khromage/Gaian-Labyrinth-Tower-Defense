using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : StatusEffect
{
    private int remainingTicks;
    private float tickRate = 2f;
    private float stunDuration;

    private bool ticking;
    private float tickStartTime;
    public Shock(float duration, float stunDuration)
    {
        this.duration = duration;
        this.stunDuration = stunDuration;
        id = DebuffList.shockID;
    }
    public Shock(float duration, float stunDuration, float tickRate)
    {
        this.duration = duration;
        this.stunDuration = stunDuration;
        this.tickRate = tickRate;
        id = DebuffList.shockID;
    }

    public override void Effect(GameObject subject, List<StatusEffect> vulnList)
    {
        if (remainingTicks > 0 && !ticking && timeElapsed > duration - remainingTicks * tickRate)
        {
            ticking = true;
            tickStartTime = timeElapsed;
            remainingTicks--;
        }
        else if (ticking && timeElapsed - tickStartTime <= stunDuration)
        {
            subject.GetComponent<EnemyBehavior>().AddMovementModifier(0f);
        }
        else
        {
            ticking = false;
        }
    }
}
