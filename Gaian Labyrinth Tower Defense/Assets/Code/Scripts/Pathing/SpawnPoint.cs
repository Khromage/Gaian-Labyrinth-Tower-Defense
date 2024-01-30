using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : GridTile
{
    //identifier for this point, for debugging purposes mostly.
    public int spawnPointNumber;

    //2D array [wave number, enemies to spawn in that wave]
    public WaveStruct[] waveSet;

    //goal of the path. manually put it into inspector for now. (select the instance on scene, and drag the instance in scene of the goal tile to this instance's inspector)
    [SerializeField]
    private GameObject endTile;

    // Start is called before the first frame update
    void Start()
    {
        placeable = false;
    }

    //invoked by OnWaveStart event from LevelManager
    //instantiates enemies based on the current wave and the listed enemies (added in inspector) for that wave.
    private void WaveStart(int waveNum)
    {
        Debug.Log($"Wave {waveNum} starting in spawnpoint script");
        StartCoroutine(spawnDelay(1f, waveNum));
    }

    IEnumerator spawnDelay(float timeToWait, int waveNum)
    {
        for (int i = 0; i < waveSet[waveNum - 1].waveEnemies.Length; i++)
        {
            GameObject currEnemy = Instantiate(waveSet[waveNum - 1].waveEnemies[i], transform.position, transform.rotation);
            EnemyBehavior currEnemyScript = currEnemy.GetComponent<EnemyBehavior>();
            currEnemyScript.currTile = this.GetComponent<GridTile>();

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

}

[System.Serializable]
public struct WaveStruct
{
    public GameObject[] waveEnemies;
}
