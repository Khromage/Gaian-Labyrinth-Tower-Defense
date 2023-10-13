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
        moveAlongPath();
    }

    void moveAlongPath()
    {
        //direction = (goalPos + myHeightOffset) - currPos; 
        //myHeightOffset = my height but rotated to the normal of the goal. Need to set that up. Matrix/vector multiplication
        //Debug.Log("path[0].pos = " + path[0].transform.position);
        Vector3 moveDirNormal = Vector3.Normalize((path[0].transform.position + new Vector3(0f, .5f, 0f)) - transform.position);

        //rotate toward where you're moving

        //movement = direction * speed;
        transform.position += moveDirNormal * moveSpeed * Time.deltaTime;
        //Debug.Log("enemy.pos = " + transform.position);
        //Debug.Log("dist = " + Vector3.Distance(transform.position, path[0].gameObject.transform.position));
        if (Vector3.Distance(transform.position, path[0].gameObject.transform.position) < .8f)
        {
            //Debug.Log("removing path[0]");
            path.RemoveAt(0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //get hit by some projectile
            //lose health
    }

}
