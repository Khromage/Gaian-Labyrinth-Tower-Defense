using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0 3 4
//1 2 5
//when tile 3 tries to grab tile 2, it instead adds tile 0 to its adjList, then grabs tiles 4 and 0 as normal
//when tile 2 tries to grab tile 1, it instead adds tile 0 to its adjList, then grabs tiles 5 and 3 as normal

public class GridTileScript : MonoBehaviour
{

    //unique identifier among all grid tiles. public get, private set.
    private (int x, int y, int z) coords;
    public (int x, int y, int z) Coords {
        get { return coords; }
        private set { coords = value; }
    }

    //list of adjacent grid tiles, for calculating the path.
    public List<GameObject> adjacentTiles { get; private set; } 

    public bool walkable = true; //whether an enemy can path through it. False when tower on it or due to unique environment.
    public bool placeable = true; //whether a tower can be placed on it. False while enemies on it or due to unique environment.


    // Start is called before the first frame update
    void Start()
    {
        adjacentTiles = new List<GameObject>();
        Coords = setCoords();
        setAdjTiles();
        writeTileInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(0f, 1f, .1f, .5f);
        Gizmos.DrawCube(transform.position - new Vector3(0f, .4f, 0f), new Vector3(1f, .2f, 1f));
    }

    
    private (int,int,int) setCoords()
    {
        return (
            (int)Mathf.Round(transform.position.x),
            (int)Mathf.Round(transform.position.y),
            (int)Mathf.Round(transform.position.z)
            );
    }

    //fills our adjacent tile list with adjacent grid tile objects.
    public void setAdjTiles()
    {
        //gets an array of all colliders we can hit within (this tile's width * .6) on the same layer as this tile (3rd parameter is a layer mask, so I bitshifted the layer)
        Collider[] adjTileColliders = Physics.OverlapSphere(transform.position, (GetComponent<BoxCollider>().size.x * .6f), (1 << gameObject.layer));
        //adds the game object of each of those colliders to our adjacentTiles list
        Debug.Log(gameObject);
        foreach (Collider col in adjTileColliders)
        {
            Debug.Log($"attempting to add col: {col}, gameObj: {col.gameObject}");
            if (col.gameObject != gameObject) 
                adjacentTiles.Add(col.gameObject);
        }
    }

    //displays the tile's coords and adjacent tile coords
    public void writeTileInfo()
    {
        Debug.Log($"Grid Tile {coords}\n    adjacent tiles:\n");
        foreach (GameObject a in adjacentTiles)
            Debug.Log($"      adj tile {a.GetComponent<GridTileScript>().coords}");
    }
     
}
