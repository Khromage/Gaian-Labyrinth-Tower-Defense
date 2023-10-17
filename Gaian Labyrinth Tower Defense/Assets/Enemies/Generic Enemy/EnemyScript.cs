using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private PathFinder pathFinder;
    public List<GridTileScript> path;

    private float moveSpeed = 3f;

    private float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 10f;
        currentHealth = maxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        if (path.Count > 0)
            moveAlongPath();
        //then event for reaching the end? reduce lives remaining, and destroy this enemy? after playing an animation, preferably.
    }

    void moveAlongPath()
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

    void OnCollisionEnter(Collision collision)
    {
        //get hit by some projectile
            //lose health
        //if currentHealth <= 0, Destroy(this)
    }

}
