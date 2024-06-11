using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public List<StatusEffect>[] StatusEffectList;

    // Start is called before the first frame update
    void Start()
    {
        StatusEffectList = new List<StatusEffect>[DebuffList.length];
        for (int i = 0; i < StatusEffectList.Length; i++)
            StatusEffectList[i] = new List<StatusEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        EffectsOnMe();
    }
    //THIS MAYBE SHOULDN'T TAKE StatusEffect as a parameter, instead taking the values and then creating a status effect in here (would need polymorphs for that)
    public void ApplyStatusEffect(StatusEffect effect)
    {
        Debug.Log("Applying Status Effect: " + effect);
        StatusEffectList[effect.id].Add(effect);
    }
    protected void EffectsOnMe()
    {
        //through the array of types
        for (int s = 0; s < StatusEffectList.Length; s++)
        {
            //through the List of each type
            for (int i = StatusEffectList[s].Count - 1; i >= 0; i--)
            {
                //Debug.Log("Affecting enemy with Status effect: " + StatusEffectList[s][i]);
                //if time-based do this
                if (StatusEffectList[s][0].duration != -1)
                {
                    StatusEffectList[s][i].timeElapsed += Time.deltaTime;
                    if (StatusEffectList[s][i].timeElapsed >= StatusEffectList[s][i].duration)
                    {
                        StatusEffectList[s].Remove(StatusEffectList[s][i]);
                    }
                }

                //THIS WILL BE DONE IN EACH INDIVIDUAL CLASS'S EFFECT CALL (Burn, Shock, etc)
                //if (s == DebuffList.burnID)
                    //add damage to runningTotal,
                //else if (s == DebuffList.shockID)
                    //add multiplicative stun dur to runningTotal,
                //else if (s == DebuffList.chillID)
                    //add multiplicative slow amount to runningTotal,

                //enact the effect for a single instance of the status type
                if (i == 0 && StatusEffectList[s].Count > 0)
                    StatusEffectList[s][i].Effect(this.gameObject, StatusEffectList[s]);
            }
        }
    }
    //or stack them up in different ways, so when applied, add to the total + calculate, then at the same ticks just apply the damage
    //
}
