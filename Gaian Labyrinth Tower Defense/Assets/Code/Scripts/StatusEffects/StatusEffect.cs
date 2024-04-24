using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ZoneEffect
{
    public float duration;
    public float timeElapsed;
    public virtual void Effect(GameObject subject, List<StatusEffect> allOfThisType)
    {

    }
}
