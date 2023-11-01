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
        //direction = normalize((goalPos + myHeightOffset) - currPos ); 
        //myHeightOffset = my height but rotated to the normal of the goal. Need to set that up. Matrix/vector multiplication

        Vector3 moveDirNormal = Vector3.Normalize((path[0].transform.position + new Vector3(0f, .5f, 0f)) - transform.position);

        //should also rotate toward where you're moving

        //movement = direction * speed;
        //Debug.Log(moveDirNormal);
        transform.position += moveDirNormal * moveSpeed * Time.deltaTime;

        //if reach next tile in path, remove it from path.
        if (Vector3.Distance(transform.position, path[0].gameObject.transform.position) < .8f)
        {
            //Debug.Log($"removing path[0]: {path[0].Coords}");
            path.RemoveAt(0);
        }
    }

    //called by tower placed event in Player. recalculates the A* path using the current tile the enemy is on and the goal tile.
    private void recalculatePath(GridTile changedTile) //parameter from TowerPlaced event. unused for now
    {
        Ray ray = new Ray(this.transform.position, -this.transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            currTile = hit.transform.GetComponent<GridTile>();
        }
        path = pathFinder.FindPath(currTile, endTile);
        if (path == null) // if no path to end, just go straight to it, for now.
        {
            path.Add(endTile.GetComponent<GridTile>());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //get hit by some projectile
            //lose health
        //if currentHealth <= 0, Destroy(this)
    }

    private void OnEnable()
    {
        Player.OnTowerPlaced += recalculatePath;
    }
    private void OnDisable()
    {
        Player.OnTowerPlaced -= recalculatePath;
    }

}
