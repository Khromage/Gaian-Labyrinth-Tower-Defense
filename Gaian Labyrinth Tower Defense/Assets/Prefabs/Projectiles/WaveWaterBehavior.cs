using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS IS BEHAVIOR OF A WATER? BASED ATTACK AND NOT RELATED TO WAVES SPAWNING ENEMIES
public class WaveWaterBehavior : MonoBehaviour
{
    public GridTile currTile;
    public LayerMask Grid;
    protected float moveSpeed = 3f;
    public float tilesLeft;
    public GridTile prevTile;
    public float damageWave; //some number
    public GameObject[] enemiesHit;
    public float NavigationCooldown;

    // Start is called before the first frame update
    void Start()
    {
        NavigationCooldown = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        NavigationCooldown -= Time.deltaTime;
        if(NavigationCooldown <= 0)
            updateCurrTile();
        moveAlongPathReverse();
    }

    //taken from enemy with slight changes
    private void moveAlongPathReverse()
    {
        Vector3 posToMoveToward = transform.position;
        if (prevTile != null)
            posToMoveToward = prevTile.transform.position;
        else
            Debug.Log("no predecessor to move toward");
        Vector3 moveDirNormal = Vector3.Normalize((posToMoveToward + new Vector3(0f, .5f, 0f)) - transform.position);

        transform.Translate(moveDirNormal * moveSpeed * Time.deltaTime);
    }

    //taken from enemy with slight changes


    private void updateCurrTile()
    {
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            
            currTile = hit.transform.GetComponent<GridTile>();
            //if not branching, keep going
            if (currTile.predecessorList.Count == 1)
                prevTile = currTile.predecessorList[0];
            //
            else if (currTile.predecessorList.Count >= 2)
            {
                //create a copy going down every other path
                for (int i = 1; i < currTile.predecessorList.Count; i++)
                {
                    bool covered = false;
                    //check every tile already traveled
                    for(int j = 0; j < tilesCovered.Count; j++)
                    {
                        if (currTile.predecessorList[i] == tilesCovered[j])
                            covered = true;
                    }
                    if (!covered)
                      Split(currTile.predecessorList[i]);
                    covered = false;
                }
                //and continue down the first index
                prevTile = currTile.predecessorList[0];
            }
            else
                Destroy(gameObject);
        }
    }

    //copied from bullet
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            foreach(gameObject i in enemiesHit)
            {
                //if enemy has been hit before(by this wave)
                if(other == i)
                    //do nothing
                    break;
                
            }
            HitTarget(other.gameObject);
            enemiesHit.append(other.gameObject);

        }
        
    }

    //create another waveattack per split path(try not to have the waves overlap and
    //super smack some enemy in the middle)
    public void Split(GridTile tile)
    {
        GameObject WaveAttack = Instantiate(WaveWater, tile, target.rotation, GameObject.transform.parent);
        WaveAttack.tilesLeft = tilesLeft;    

    }
}
