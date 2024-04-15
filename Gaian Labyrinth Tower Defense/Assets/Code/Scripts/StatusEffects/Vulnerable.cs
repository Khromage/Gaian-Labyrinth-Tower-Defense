using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulnerable : StatusEffect
{
    private float amplification;

    //Zone
    public Vulnerable(float amplification)
    {
        duration = -1f;
        this.amplification = amplification;
    }
    //Duration
    public Vulnerable(float duration, float amplification)
    {
        this.duration = duration;
        this.amplification = amplification;
    }

    public override void Effect(EnemyBehavior subject)
    {
        subject.isVulnerable = true;
        if (amplification > subject.vulnMulti)
        {
            subject.vulnMulti = amplification;
        }
    }
}
