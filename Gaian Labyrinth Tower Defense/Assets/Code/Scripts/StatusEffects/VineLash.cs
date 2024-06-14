using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineLash : StatusEffect
{
    public float dmgPerLash;
    
    public float range;
    public float width;
    public float speedOfLash;

    public float duration;
    public float timeElapsed;
    //public float remainingTicks;
    public GameObject projectilePrefab;

    public VineLash(float duration, float dmgPerLash, float range, float width, float speedOfLash)
    {
        this.duration = duration;
        this.dmgPerLash = dmgPerLash;
        this.range = range;
        this.width = width;
        this.speedOfLash = speedOfLash;
        timeElapsed = 0f;
        Debug.Log("New VineLash created success");
        //BuffList when?
        //id = DebuffList.vineLashID;
    }

    public virtual void Effect(GameObject enemy, List<StatusEffect> allOfThisType)
    {
        if (enemy.tag == "enemies")
        {
            EnemyBehavior e = enemy.GetComponent<EnemyBehavior>();


            VineLashBehavior projectile = Instantiate(projectilePrefab, enemy.transform.position, enemy.transform.rotation).GetComponent<VineLashBehavior>();
            projectile.damage = dmgPerLash;
            projectile.target = enemy;
            projectile.range = range;
            projectile.width = width;
            projectile.speed = speedOfLash;


            Debug.Log("Lashing Out!");
            //damage dealing might be attached to a summon lash, kinda like wave
            //t.target.takeDamage(dmgPerLash);
            //remainingTicks--;

        }
        else
        {
            Debug.Log("Trying to give vines to non-tower");
        }
    }

}
