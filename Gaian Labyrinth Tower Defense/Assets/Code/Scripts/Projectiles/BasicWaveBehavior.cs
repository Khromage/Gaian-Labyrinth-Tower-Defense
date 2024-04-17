using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWaveBehavior : MonoBehaviour
{
    public GridTile currTile;
    public LayerMask Grid;
    protected float moveSpeed = 4f;
    public float tilesLeft;
    public GridTile nextTile;
    public float damage; //some number
    //public GameObject WavePrefab;
    //WaveMaster WaveMaster;
    public bool lowLevel;

    void Start()
    {
        //WaveMaster = transform.parent.gameObject.GetComponent<WaveMaster>();
        Destroy(gameObject, 5);

        //temp
        tilesLeft = 5;
    }
    void Update()
    {
        updateCurrTile();
        moveAlongPathReverse();
        if (tilesLeft <= 0)
            Destroy(gameObject);
    }
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

    private void updateCurrTile()
    {

        Ray ray = new Ray(this.transform.position, -this.transform.up);


        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            

            //if not branching, keep going
            if (currTile.predecessorList.Count == 1)
            {
                nextTile = currTile.predecessorList[0];
                Debug.Log(nextTile);
            
            //
            
            }
            else
                Destroy(gameObject);
        }
        else
        {
            Debug.Log("i missed the ground");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {


            
                //EnemyBehavior e = other.GetComponent<EnemyBehavior>();
            other.GetComponent<EnemyBehavior>().takeDamage(damage, gameObject);
            if (lowLevel == true)
                Destroy(gameObject);
            


        }

    }


}
