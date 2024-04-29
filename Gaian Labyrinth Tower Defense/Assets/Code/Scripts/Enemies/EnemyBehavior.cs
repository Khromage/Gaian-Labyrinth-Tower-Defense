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
    public float dmgMulti;

    // keeps track of the number of zones the enemy is inside (in case tower ranges overlap)
    private int buffZones;
    public bool isBuffed;


    protected List<float> moveSpeedModifiers;
    protected List<float> damageModifiers;

    public GameObject damageIndicator;

    private Camera cameraToWatch;

    //currency gain on death
    public int worth;

    //damage to core/remaining lives
    public int harm;

    //public float value? for when it dies
    [SerializeField]
    protected AudioSource EnemyHurtSFX;


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

        moveSpeedModifiers = new List<float>();
        damageModifiers = new List<float>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        ApplyMovementModifiers();
        ApplyDamageModifiers();
        //setGravityDir();
        //updateCurrTile();
        //moveAlongPath();
        
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
        float finalDamage = damage * dmgMulti;
        Debug.Log("dmgMulti: " + dmgMulti);

        currentHealth -= finalDamage;

        string printMsg = "Enemy took " + damage + "damage. Final damage was " + finalDamage + ". Vulnerable: ";
        if(isVulnerable)
        {
            printMsg += "TRUE";
        } else {
            printMsg += "FALSE";
        }
        if (isBuffed)
        {
            printMsg += " Buffed: TRUE";
        }
        else
        {
            printMsg += " Buffed: FALSE"; 
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
        if (isVulnerable || isBuffed)
            finalDamage *= dmgMulti;

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
        if (isBuffed)
        {
            printMsg += " Buffed: TRUE";
        }
        else
        {
            printMsg += " Buffed: FALSE"; 
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

    public void AddDamageModifier(float mod)
    {
        damageModifiers.Add(mod);
    }
    protected void ApplyDamageModifiers()
    {
        float totalModifier = 1f;
        foreach (float m in damageModifiers)
        {
            totalModifier *= m;
            //Debug.Log("Modiier before: " + totalModifier);
            dmgMulti = totalModifier;
        }
        //Debug.Log("Modiier after: " + totalModifier);
        damageModifiers.Clear();
        
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

    public void enterBuffZone(int dmgMod)
    {
        buffZones++;
        isBuffed = true;
    }

    public void exitBuffZone()
    {
        buffZones--;
        if(buffZones < 1)
        {
            isBuffed = false;
        }
        dmgMulti = 1f;
    }
}
