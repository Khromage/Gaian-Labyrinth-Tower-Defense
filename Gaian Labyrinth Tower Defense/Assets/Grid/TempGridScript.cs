using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//just here to instantiate the grid. Not permanent. In more complicated levels will probably just place them manually

public class TempGridScript : MonoBehaviour
{
    public GameObject gridTile;
    public GameObject enemy1;

    private PathFinder pathFinder;
    public List<GridTileScript> path;

    private GridTileScript startTile = null;
    private GridTileScript endTile = null;


    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("grid starting");
        pathFinder = new PathFinder();
        //calculate path
        //at start of each wave, re-calculate path (later make it so it recalculates on tower placement on path, or any towers being sold)


        (startTile, endTile) = spawnGrid();
        path = pathFinder.FindPath(startTile, endTile);
        string pathString = "";
        foreach(GridTileScript s in path)
        {
            Debug.Log("pathElement: " + s);
            pathString += " " + s.Coords;
        }
        Debug.Log("Path: " + pathString);
        spawnWave();

    }

    public void spawnWave()
    {
        Debug.Log("spawning enemy");
        GameObject currEnemy = Instantiate(enemy1, startTile.gameObject.transform.position, transform.rotation);
        currEnemy.GetComponent<EnemyScript>().path = path;
    }


    private (GridTileScript, GridTileScript) spawnGrid()
    {
        GridTileScript start = null;
        GridTileScript end = null;
        List<GridTileScript> tileScriptList = new List<GridTileScript>();
        for (int i = -25; i < 25; i++)
        {
            for (int j = -25; j < 25; j++)
            {
                GameObject currTile = Instantiate(gridTile, new Vector3(transform.position.x + i + .49f, transform.position.y, transform.position.z + j + .49f), transform.rotation);
                if (j < i - 5 || j > i + 5)
                    currTile.GetComponent<GridTileScript>().walkable = false;

                tileScriptList.Add(currTile.GetComponent<GridTileScript>());

                if (i == -20 && j == -20)
                {
                    start = currTile.GetComponent<GridTileScript>();
                    //start tile
                    Debug.Log($"Found start tile: {start.gameObject.transform.position}");
                }
                if (i == 20 && j == 20) {
                    end = currTile.GetComponent<GridTileScript>();
                    //end tile
                    Debug.Log($"Found end tile: {end.gameObject.transform.position}");
                }
            }
        }
        foreach (GridTileScript gts in tileScriptList)
        {
            gts.setAdjTiles();
        }

        return (start, end);
    }
}
