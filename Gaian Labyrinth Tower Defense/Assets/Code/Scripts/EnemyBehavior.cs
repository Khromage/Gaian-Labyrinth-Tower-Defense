using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//currentTile
//goalTile = path.end  (set by spawnPoint when it gives the initial path? but there might not be a path when a wave starts if it's all blocked off...)
//when a tower is placed

public class EnemyBehavior : UnitBehavior
{
    public delegate void EnemyDeath(GameObject deadEnemy);
    public static event EnemyDeath OnEnemyDeath;

    public delegate void EnemyReachedGoal(int harm);
    public static event EnemyReachedGoal OnEnemyReachedGoal;

    public GridTile currTile;
    public GridTile successorTile;
    public LayerMask Grid;
    
    public float moveSpeed = 3f;
    private float maxHealth;
    public float currentHealth;

    //currency gain on death
    public int worth;

    //damage to core/remaining lives
    public int harm;

    //Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        gameObject.GetComponent<ConstantForce>().force = defaultGravityDir * rb.mass * gravityConstant;
        rb.drag = 1f;

        harm = 1;
        worth = 5;
        maxHealth = 12f;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        updateCurrTile();
        
        if (currTile is GoalTile)
        {
            Debug.Log("reached end, presumably");
            OnEnemyReachedGoal?.Invoke(harm);
            OnEnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
        if (currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        moveAlongPath();
    }

    public void takeDamage(float damage, GameObject damagerBullet)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    private void moveAlongPath()
    {
        //direction = normalize((successorPos + myHeightOffset) - currPos ); 
        //myHeightOffset = my height but rotated to the normal of the goal. Need to set that up. Matrix/vector multiplication
        Vector3 posToMoveToward = transform.position;
        if (successorTile != null)
            posToMoveToward = successorTile.transform.position;
        else
            Debug.Log("enemy no successor to move toward");
        Vector3 moveDirNormal = Vector3.Normalize((posToMoveToward + new Vector3(0f, .5f, 0f)) - transform.position);

        //Debug.Log(moveDirNormal * moveSpeed * 5f);
        if (rb.velocity.magnitude < moveSpeed)
            rb.AddForce(moveDirNormal * moveSpeed * 10f, ForceMode.Force);
        //rb.drag = rb.velocity.magnitude / moveSpeed;
        //Debug.Log($"currDrag: {rb.drag}");
        //transform.Translate(moveDirNormal * moveSpeed * Time.deltaTime);

        //should also rotate toward where you're moving
    }

    private void updateCurrTile()
    {
        //get current tile. Might adjust this to check less often than on every frame.
        Ray ray = new Ray(this.transform.position + this.transform.up, -this.transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            //Debug.Log("hitting tile");
            currTile = hit.transform.GetComponent<GridTile>();
            successorTile = currTile.successor;
            currTile.enemyOnTile = true;
        }
    }

}
