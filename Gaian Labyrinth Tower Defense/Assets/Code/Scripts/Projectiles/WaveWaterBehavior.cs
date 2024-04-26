using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS IS BEHAVIOR OF THE WATER? BASED ATTACK AND NOT RELATED TO WAVES SPAWNING ENEMIES
//and how it traverses
public class WaveWaterBehavior : MonoBehaviour
{
    public GridTile currTile;
    public LayerMask Grid;
    protected float moveSpeed = 4f;
    public float tilesLeft;
    public GridTile nextTile;
    public float damage; //some number
    public GameObject WavePrefab;
    WaveMaster WaveMaster;

    // Start is called before the first frame update
    void Start()
    {
        WaveMaster = transform.parent.gameObject.GetComponent<WaveMaster>();
        Destroy(gameObject, 5);
        //Debug.Log("wave created");
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrTile();
        moveAlongPathReverse();
        //Debug.Log("i travel for " + tilesLeft + " tiles");
        if(tilesLeft <= 0)
            Destroy(gameObject);
    }

    //taken from enemy with slight changes
    private void moveAlongPathReverse()
    {
        Vector3 posToMoveToward = transform.position;
        if (nextTile != null)
            posToMoveToward = nextTile.transform.position;
        else
            Debug.Log("no predecessor to move toward");
        Vector3 moveDirNormal = Vector3.Normalize(posToMoveToward - transform.position);

        transform.Translate(moveDirNormal * moveSpeed * Time.deltaTime);
    }

    //taken from enemy with slight changes
    private void updateCurrTile()
    {

        Ray ray = new Ray(this.transform.position, -this.transform.up);

        
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            List<GridTile> TilesCovered = WaveMaster.tilesCovered;
            currTile = hit.transform.GetComponent<GridTile>();
            if(TilesCovered == null)
                WaveMaster.tilesCovered.Add(currTile);
            if(!TilesCovered.Contains(currTile))
            {
                WaveMaster.tilesCovered.Add(currTile);
                tilesLeft -= 1;
            }
        
            //if not branching, keep going
            if (currTile.predecessorList.Count == 1)
            {
                nextTile = currTile.predecessorList[0];
            }
            //
            else if (currTile.predecessorList.Count >= 2)
            {
                //for each branch except the first
                for (int i = 1; i < currTile.predecessorList.Count; i++)
                {
                    bool covered = false;
                    //check every tile already traveled
                    for(int j = 0; j < TilesCovered.Count; j++)
                    {
                        if (currTile.predecessorList[i] == TilesCovered[j])
                            covered = true;
                    }
                    //create a clone going that path
                    if (!covered)
                      Split(currTile.predecessorList[i]);
                    covered = false;
                }
                //and continue down the first index
                nextTile = currTile.predecessorList[0];
            }
            else
                Destroy(gameObject);
        }
        else
        {
            Debug.Log("i missed the ground");
        }
    }

    //copied from bullet
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            if(!WaveMaster.enemiesHit.Contains(other.gameObject))
            {
                HitTarget(other.gameObject);
                WaveMaster.enemiesHit.Add(other.gameObject);
            }

        }
        
    }
    //copied bullet behavior
    private void HitTarget(GameObject hitEnemy)
    {
        EnemyBehavior e = hitEnemy.GetComponent<EnemyBehavior>();
        e.takeDamage(damage, gameObject);  
    }

    //create another miniwave per split path(try not to have the waves overlap and
    //super smack some enemy in the middle)
    public void Split(GridTile tile)
    {
        Vector3 direction = tile.transform.position - transform.position;
        Quaternion SplitDirection = Quaternion.Euler(direction);
        GameObject branchWave = Instantiate(WavePrefab, tile.transform.position, SplitDirection, gameObject.transform.parent);
        WaveMaster.waterWaves.Add(branchWave);
        branchWave.GetComponent<WaveWaterBehavior>().tilesLeft = tilesLeft;

    }
}
