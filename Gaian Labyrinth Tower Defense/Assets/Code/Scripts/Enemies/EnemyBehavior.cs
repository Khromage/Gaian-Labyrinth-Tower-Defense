using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

//currentTile
//goalTile = path.end  (set by spawnPoint when it gives the initial path? but there might not be a path when a wave starts if it's all blocked off...)
//when a tower is placed

public class EnemyBehavior : MonoBehaviour
{
    public delegate void EnemyDeath(EnemyBehavior deadEnemy);
    public event EnemyDeath OnEnemyDeath;

    public delegate void EnemyReachedGoal(EnemyBehavior enemy);
    public event EnemyReachedGoal OnEnemyReachedGoal;

    public GridTile currTile;
    public GridTile successorTile;
    public LayerMask Grid;
    public float moveSpeed;
    protected bool isAlive;

    [SerializeField]
    protected EnemyHealthBar HealthBar;
    protected int enemyID;
    protected float maxHealth;
    public float currentHealth;
    public enum Weight { light, medium, heavy };
    public Weight enemyWeight;

    // keeps track of the number of zones the enemy is inside (in case tower ranges overlap)
    private int vulnerabilityZones;
    public bool isVulnerable;
    public float vulnMulti;

    public GameObject damageIndicator;

    private Camera cameraToWatch;

    //currency gain on death
    public int worth;

    //damage to core/remaining lives
    public int harm;

    //public float value? for when it dies
    [SerializeField]
    protected AudioSource EnemyHurtSFX;

    //protected List<StatusEffect> StatusEffectList;
    protected List<StatusEffect>[] StatusEffectList;
    protected List<float> moveSpeedModifiers;

    [SerializeField]
    private EnemyInfo info;

    //public Vector3 posToMoveToward;

    // Start is called before the first frame update
    public virtual void Start()
    {
        //change these to pull from the scriptableObject
        harm = info.harm;
        worth = info.worth;
        maxHealth = info.maxHealth;
        moveSpeed = info.moveSpeed;
        enemyWeight = info.currentWeight;
        isAlive = true;
        isVulnerable = false;
        vulnerabilityZones = 0;
        currentHealth = maxHealth;
        EnemyHurtSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        EffectsOnMe();
        ApplyMovementModifiers();
        //setGravityDir();
        updateCurrTile();
        moveAlongPath();
        
        if (currTile is GoalTile)
        {
            OnEnemyReachedGoal?.Invoke(this);
            isAlive = false;
        }

        if(!isAlive)
        {
            OnEnemyDeath?.Invoke(this);
        }
    }

    void LateUpdate()
    {
        if(!isAlive)
        {
            Destroy(gameObject);
            Destroy(HealthBar.gameObject);
        }
    }
    public void takeDamage(float damage, GameObject damagerBullet)
    {
        float finalDamage = damage;
        if(isVulnerable)
            finalDamage *= vulnMulti;

        currentHealth -= finalDamage;

        string printMsg = "Enemy took " + damage + "damage. Final damage was " + finalDamage + ". Vulnerable: ";
        if(isVulnerable)
        {
            printMsg += "TRUE";
        } else {
            printMsg += "FALSE";
        }
        Debug.Log(printMsg);

        deployDamageIndicator(finalDamage);

        float spd = 5 + 5 * finalDamage / maxHealth;
        HealthBar.SetHealth(currentHealth / maxHealth, spd);

        EnemyHurtSFX.Play();
        if(currentHealth <= 0)
        {
            isAlive = false;
        }
        
    }
    //polymorph without the bullet object...
    public void takeDamage(float damage)
    {
        float finalDamage = damage;
        if (isVulnerable)
            finalDamage *= vulnMulti;

        currentHealth -= finalDamage;

        string printMsg = "Enemy took " + damage + "damage. Final damage was " + finalDamage + ". Vulnerable: ";
        if (isVulnerable)
        {
            printMsg += "TRUE";
        }
        else
        {
            printMsg += "FALSE";
        }
        Debug.Log(printMsg);

        deployDamageIndicator(finalDamage);

        float spd = 5 + 5 * finalDamage / maxHealth;
        HealthBar.SetHealth(currentHealth / maxHealth, spd);

        EnemyHurtSFX.Play();
        if (currentHealth <= 0)
        {
            isAlive = false;
        }

    }

    private void deployDamageIndicator(float damage)
    {
        GameObject dmgInd = Instantiate(damageIndicator, gameObject.transform);
        Destroy(dmgInd, 2f);
        TMP_Text dmgIndText = dmgInd.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        //dmgInd.GetComponent<ConstantForce>.force = currGravDir;
        dmgIndText.text = damage.ToString();

        if (damage >= 3)
            dmgIndText.color = Color.yellow;
        if (damage >= 10)
            dmgIndText.color = Color.red;

        if (dmgIndText.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
            faceCamera.Camera = cameraToWatch;

        //dmgInd.GetComponent<Rigidbody>().AddForce(new Vector3(Vector3.Dot(Vector3.right, transform.right) * UnityEngine.Random.value, 10f, Vector3.Dot(Vector3.forward, transform.forward) * UnityEngine.Random.value), ForceMode.Impulse);
        dmgInd.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.value * 3f - .5f, 12f, UnityEngine.Random.value * 3f - .5f), ForceMode.Impulse);
    }

    private void moveAlongPath()
    {
        //direction = normalize((successorPos + myHeightOffset) - currPos ); 
        //myHeightOffset = my height but rotated to the normal of the goal. Need to set that up. Matrix/vector multiplication
        Vector3 posToMoveToward = transform.position;
        if (successorTile != null)
            posToMoveToward = successorTile.transform.position;
        /*
        else
            Debug.Log("enemy no successor to move toward");
        */
        Vector3 moveDirNormal = Vector3.Normalize((posToMoveToward + new Vector3(0f, .5f, 0f)) - transform.position);

        transform.Translate(moveDirNormal * moveSpeed * Time.deltaTime);
        //if (rb.velocity.magnitude < moveSpeed)
        //    rb.AddForce(moveDirNormal * moveSpeed * 10f, ForceMode.Force);

        //should also rotate toward where you're moving
    }

    private void updateCurrTile()
    {
        //get current tile. Might adjust this to check less often than on every frame.
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            //Debug.Log("hitting tile");
            currTile = hit.transform.GetComponent<GridTile>();
            successorTile = currTile.successor;
            currTile.enemyOnTile = true;
        }
    }


    protected void EffectsOnMe()
    {
        //throught the array of types
        for (int s = 0; s < StatusEffectList.Count(); s++) {
            //through the List of each type
            for (int i = StatusEffectList[s].Count - 1; i >= 0; i--)
            {

                //if time-based do this
                if (StatusEffectList[s][0].duration != -1)
                {
                    StatusEffectList[s][i].timeElapsed += Time.deltaTime;
                    if (StatusEffectList[s][i].timeElapsed >= StatusEffectList[s][i].duration)
                    {
                        StatusEffectList[s].Remove(StatusEffectList[s][i]);
                    }
                }

                StatusEffectList[s][i].Effect(this);
            }
        }
    }

    public void AddMovementModifier(float mod)
    {
        moveSpeedModifiers.Add(mod);
    }
    protected void ApplyMovementModifiers()
    {
        float totalModifier = 1f;
        foreach (float m in moveSpeedModifiers)
        {
            totalModifier *= m;
        }
        moveSpeedModifiers.Clear();
        GetComponent<NavMeshAgent>().speed = moveSpeed * totalModifier;
    }
    //THIS MAYBE SHOULDN'T TAKE StatusEffect as a parameter, instead taking the values and then creating a status effect in here (would need polymorphs for that)
    public void ApplyStatusEffect(int id, float dur, float val)
    {
        //StatusEffectList[id].Add(DebuffList.newDebuff(id, dur, val));
        
        /*
        public T newDebuff<T>(int id, float dur, float val)
        {
            switch (id)
            {
                case 0:
                    return Burn;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default: 
                    break;
            }
        }
        */
    }
    public void ApplyStatusEffect(int id, float dur, float val, float rate)
    {
        //StatusEffectList[id].Add(statusEffect);
    }


    //OBSOLETE
    public void enterVulnerabilityZone()
    {
        vulnerabilityZones++;
        // Debug.Log("enemy entered arcane zone. enemy is inside of " + vulnerabilityZones + "arcane vuln zones");
        isVulnerable = true;
    }
    public void exitVulnerabilityZone()
    {
        vulnerabilityZones--;
        if(vulnerabilityZones < 1)
        {
            isVulnerable = false;
            // Debug.Log("No more vuln zones, isVulnerable being set to false");
        }
    }
}
