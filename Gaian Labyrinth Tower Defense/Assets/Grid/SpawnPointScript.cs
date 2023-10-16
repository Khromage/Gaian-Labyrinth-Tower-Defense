using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : GridTileScript
{
    //identifier for this point, for debugging purposes mostly.
    public int spawnPointNumber;

    //2D array [wave number, enemies to spawn in that wave]
    public WaveStruct[] waveSet;

    //Path for enemies that spawn here.
    private PathFinder pathFinder;
    public List<GridTileScript> path;

    //goal of the path. manually put it into inspector for now. (select the instance on scene, and drag the instance in scene of the goal tile to this instance's inspector)
    [SerializeField]
    private GameObject endTile;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"spawn point {spawnPointNumber} starting");
        pathFinder = new PathFinder();

        //at start of each wave, re-calculate path (later make it so it recalculates on tower placement on path, or any towers being sold)
        //might want a list or dictionary of every tile, then can grab any with "endpoint" flag or w/e, and pick the closest one for this next line
        //takes scripts of start tile and end tile.
        path = pathFinder.FindPath(this, endTile.GetComponent<GridTileScript>());

        printPath();
    }


    //invoked by OnWaveStart event from LevelManager
    //instantiates enemies based on the current wave and the listed enemies (added in inspector) for that wave.
    private void WaveStart(int waveNum)
    {
        Debug.Log("Wave starting in spawnpoint script");
        StartCoroutine(spawnDelay(1f, waveNum));
    }

    IEnumerator spawnDelay(float timeToWait, int waveNum)
    {
        for (int i = 0; i < waveSet[waveNum - 1].waveEnemies.Length; i++)
        {
            GameObject currEnemy = Instantiate(waveSet[waveNum - 1].waveEnemies[i], transform.position, transform.rotation);
            currEnemy.GetComponent<EnemyScript>().path = new List<GridTileScript>(path);
            Debug.Log($"enemy {i} pos: {currEnemy.transform.position}");

            Debug.Log($"stalling in spawnDelay for {timeToWait} sec");
            yield return new WaitForSeconds(timeToWait);
        }
    }



    void OnDrawGizmos()
    {
        // Draw a semitransparent red box at the transforms position
        Gizmos.color = new Color(1f, 0f, 0f, .5f);
        Gizmos.DrawCube(transform.position - new Vector3(0f, .4f, 0f), new Vector3(1f, .2f, 1f));
    }

    private void OnEnable()
    {
        LevelManager.OnWaveStart += WaveStart;
    }
    private void OnDisable()
    {
        LevelManager.OnWaveStart -= WaveStart;
    }

    //checking path in console.
    private void printPath()
    {
        string pathString = "";
        foreach (GridTileScript s in path)
        {
            //Debug.Log("pathElement: " + s);
            pathString += " " + s.Coords;
        }
        Debug.Log($"Path from spawn point {spawnPointNumber}: {pathString}");
    }
}

[System.Serializable]
public struct WaveStruct
{
    public GameObject[] waveEnemies;
}
