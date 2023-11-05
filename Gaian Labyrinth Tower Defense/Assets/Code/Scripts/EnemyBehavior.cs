using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//currentTile
//goalTile = path.end  (set by spawnPoint when it gives the initial path? but there might not be a path when a wave starts if it's all blocked off...)
//when a tower is placed

public class EnemyBehavior : MonoBehaviour
{
    public delegate void EnemyDeath(GameObject deadEnemy);
    public static event EnemyDeath OnEnemyDeath;

    public delegate void EnemyReachedGoal(int harm);
    public static event EnemyReachedGoal OnEnemyReachedGoal;

    private PathFinder pathFinder;
    public List<GridTile> path;
    public GridTile currTile;
    public GridTile endTile;
    public LayerMask Grid;
    
    private float moveSpeed = 3f;
    private float maxHealth;
    public float currentHealth;

    //currency gain on death
    public int worth;

    //damage to core/remaining lives
    public int harm;

    //public float value? for when it dies

    // Start is called before the first frame update
    void Start()
    {
        harm = 1;
        worth = 5;
        maxHealth = 10f;
        currentHealth = maxHealth;
        pathFinder = new PathFinder();
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count > 0)
            moveAlongPath();
        else
        {
            Debug.Log("reached end, presumably");
            OnEnemyReachedGoal?.Invoke(harm);
            OnEnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
        //then event for reaching the end? reduce lives remaining, and destroy this enemy? after playing an animation, preferably.
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
        //get current tile. Might adjust this to check less often than on every frame.
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            Debug.Log("hitting tile");
            currTile = hit.transform.GetComponent<GridTile>();
        }

        //direction = normalize((successorPos + myHeightOffset) - currPos ); 
        //myHeightOffset = my height but rotated to the normal of the goal. Need to set that up. Matrix/vector multiplication
        Vector3 posToMoveToward = currTile.successor.transform.position;
        Vector3 moveDirNormal = Vector3.Normalize((posToMoveToward + new Vector3(0f, .5f, 0f)) - transform.position);

        transform.Translate(moveDirNormal * moveSpeed * Time.deltaTime);

        //should also rotate toward where you're moving
    }


    void OnCollisionEnter(Collision collision)
    {

    }

    private void OnEnable()
    {
        //Player.OnTowerPlaced += recalculatePath;
    }
    private void OnDisable()
    {
        //Player.OnTowerPlaced -= recalculatePath;
    }

}
