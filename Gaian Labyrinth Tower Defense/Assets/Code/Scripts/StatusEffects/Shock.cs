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
        remainingTicks = (int)(duration * tickRate);
        id = DebuffList.shockID;
    }
    public Shock(float duration, float stunDuration, float tickRate)
    {
        this.duration = duration;
        this.stunDuration = stunDuration;
        this.tickRate = tickRate;
        remainingTicks = (int)(duration * tickRate);
        id = DebuffList.shockID;
    }

    public override void Effect(GameObject subject, List<StatusEffect> shockList)
    {
        //Debug.Log($"Shock effect. remainingTicks:{remainingTicks},  ticking:{ticking}");
        //Debug.Log($"timeElapsed:{timeElapsed},  time of next tick:{duration - remainingTicks / tickRate}");
        if (remainingTicks > 0 && !ticking && timeElapsed > duration - remainingTicks / tickRate)
        {
            ticking = true;
            tickStartTime = timeElapsed;
            remainingTicks--;
            Debug.Log("Shock is beginning paralyse tick");
        }
        else if (ticking && timeElapsed - tickStartTime <= stunDuration)
        {
            Debug.Log("Shock trying to paralyse the enemy!");
            subject.GetComponent<EnemyBehavior>().AddMovementModifier(0f);
        }
        else
        {
            ticking = false;
            Debug.Log("Shock is not paralysing");
        }
    }
}
