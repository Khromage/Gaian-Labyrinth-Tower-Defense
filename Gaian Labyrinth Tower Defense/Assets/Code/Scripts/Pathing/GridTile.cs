using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridTile : MonoBehaviour
{

    //coords = unique identifier among all grid tiles. public get, private set.
    private (int x, int y, int z) coords;
    public (int x, int y, int z) Coords {
        get { return coords; }
        private set { coords = value; }
    }

    //distance from goal
    public int goalDist;
    public TMP_Text goalDistText;

    //list of adjacent grid tiles, for calculating the path.
    public List<GridTile> adjacentTiles { get; private set; }

    public GridTile successor;
    public List<GridTile> predecessorList;
    public bool fielded;


    //A* algorithm things. G is distance from start, H is distance from end, F is the total of both.
    //distance isn't actual distance, but Manhattan distance (so cardinal directions only)
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public GridTile previous;


    public bool walkable = true; //whether an enemy can path through it. False when tower on it or due to unique environment.
    public bool placeable = true; //whether a tower can be placed on it. False while enemies on it or due to unique environment.
    public bool enemyOnTile = false;
    public bool towerOnTile = false;


    void Awake()
    {
        goalDist = int.MaxValue;
        fielded = false;
        Coords = setCoords();
        adjacentTiles = new List<GridTile>();
        setAdjTiles();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyOnTile = false;
        //goalDistText.text = $"{goalDist.ToString()}";
        //goalDist++;
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent green box at the transforms position
        if (walkable)
            Gizmos.color = new Color(0f, 1f, .1f, .5f);
        else
            Gizmos.color = new Color(.4f, .6f, .1f, .5f);

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

        foreach (Collider col in adjTileColliders)
        {
            //Debug.Log($"attempting to add col: {col}, gameObj: {col.gameObject}");
            if (col.gameObject != gameObject) 
                adjacentTiles.Add(col.gameObject.GetComponent<GridTile>());
        }
    }

    public GridTile recalcSuccessor()
    {
        GridTile newSuccessor = successor;
        
        foreach (GridTile adjTile in adjacentTiles)
            if (adjTile.walkable && adjTile.goalDist < newSuccessor.goalDist)
                newSuccessor = adjTile;
        //if nothing changed and the successor was unwalkable
        if (!successor.walkable && newSuccessor == successor)
            newSuccessor = null;
        return newSuccessor;
    }

    //displays the tile's coords and adjacent tile coords
    public void writeTileInfo()
    {
        Debug.Log($"Grid Tile {coords}\n    adjacent tiles:\n");
        foreach (GridTile a in adjacentTiles)
            Debug.Log($"      adj tile {a.coords}");
    }
     
}
