using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CamelionBehavior : EnemyBehavior
{
    //Should remain in same state for a certain amount of time (rn 12)
    public float timeToChangeTimer = 12;
    public float timeToChange = 0;
    protected ResistState currentState;
    public StatusEffects StatusEffects;
    
    public enum ResistState{
        Normal,
        Burn,
        Chill,
        Shock, 
        Wet
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        StatusEffects = GetComponent<StatusEffects>();
        currentState = ResistState.Normal;

        if (StatusEffects != null)
        {
            StatusEffects.OnStatusEffectApplied += HandleStatusEffectApplied;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (timeToChange >0){
            timeToChange -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (StatusEffects != null)
        {
            StatusEffects.OnStatusEffectApplied -= HandleStatusEffectApplied;
        }
    }

    private void HandleStatusEffectApplied(StatusEffect effect)
    {
        if (timeToChange > 0){
            return;
        }else{
            setStatusState(effect);
        }
    }

    private void setStatusState(StatusEffect effect){
        switch (effect.id){
            case 1:
                currentState = ResistState.Burn;
                break;
            case 2:
                currentState = ResistState.Chill;
                break;
            case 3:
                currentState = ResistState.Shock;
                break;
            case 4:
                currentState = ResistState.Wet;
                break;
            default:
                currentState = ResistState.Normal;
                return;
        }
        timeToChange = timeToChangeTimer;
    }

    public override void takeDamage(float damage, GameObject damagerBullet)
    {
        string damageType = damagerBullet.GetComponent<BulletBehavior>().elementType;
        switch (currentState){
            case ResistState.Burn:
                //If bullet is applying status effect of burn, damage should be halfed
                if (damageType == "burn"){
                    damage = damage / 2;
                }
                break;
            case ResistState.Chill:
                if (damageType == "chill"){
                    damage = damage / 2;
                }
                break;
            case ResistState.Shock:
                if (damageType == "shock"){
                    damage = damage / 2;
                }
                break;
            case ResistState.Wet:
                if (damageType == "wet"){
                    damage = damage / 2;
                }
                break;
            default:
                //Should never reach here just move onto take normal damage
                break;
        }
        base.takeDamage(damage, damagerBullet);
    }





}
