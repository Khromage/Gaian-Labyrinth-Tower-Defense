using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wave start as an event? invoke

public class LevelManager : MonoBehaviour
{
    //Wave start event. Invoked on every new wave. SpawnPoints (and potentially other classes) will use the event.
    public delegate void WaveStart(int waveNum);
    public static event WaveStart OnWaveStart;
    
    //Timer
    private float waveTimer = 10f; //total time between waves
    public float waveCountdown; //currently remaining time. Display this.

    public int currWave = 0;

    [SerializeField] private GameObject goalTile;
    public FlowFieldGenerator flowFieldGenerator;

    private GameObject[] goalTileSet;

    //[SerializeField]
    public int remainingLives = 20;
    //maybe an event where an enemy reaches the goal? invoked by the enemy, then in this script adjust remainingLives

    //spawnPoint list
        //FindGameObjectWithName? SpawnPointSet, and grab all of its children for our list to use for the waves.
        //or manually fill the list with the spawn point objects back in the inspector for each level
    //gameMode? (difficulty?)

    // Start is called before the first frame update
    void Start()
    {
        goalTileSet = getGoalTiles();

        flowFieldGenerator = new FlowFieldGenerator();
        flowFieldGenerator.GenerateField(goalTileSet[0].GetComponent<GridTile>(), 0);


        waveCountdown = 1f;
        remainingLives += 5;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currWave < 7) //currWave < # of waves.
        {
            //Gameplay/design decision: maybe wait to start countdown until wave has been defeated, or just until they've all spawned. 
            waveCountdown -= Time.deltaTime;

            if (waveCountdown <= 0)
            {
                waveCountdown = waveTimer;
                currWave++;
                waveTimer++;
                OnWaveStart?.Invoke(currWave);
            }
        }
    }

    private void LoseLives(int harm)
    {
        Debug.Log("Losing lives in LevelManager");
        remainingLives -= harm;
        Debug.Log(harm);
        Debug.Log(remainingLives);
    }


    //currently does a full Field Generation. Maybe can be made more efficient
    private void recalcFlowField_NewTower(GridTile towerTile)
    {
        Debug.Log("tower placed event in level manager. recalcing field");

        flowFieldGenerator.GenerateField(goalTile.GetComponent<GridTile>(), 0);

        /*
        thinking of doing A* from the tower tile's predecessor until it finds tiles with lower goalDist than the tower tile,
        then from there GenerateField on part of the map.
        Would have to re-generate from a minimum any time you come across a shorter distance though? Any time you come across a distance more than 1 shorter.
        imagine A* to goal, but we stop once we find a tile with a shorter goalDist than the tile the tower was placed on
        then from there we GenerateField over part of the map flowing from there
        Any time we come across a better route (so any adj tiles with >1 goalDist shorter than currTile we're on at the frontier), that better route/tile starts a new GenerateField
        so potentially several GenerateFields over part of the map, or potentially just one. That on top of partway A* to start, maybe not much better than a full GenerateField
        */
    }

    //does a partial Field Generation, starting from the opening created by the absent tower, rather than a full Field Generation
    private void recalcFlowField_NewTile(GridTile newTile)
    {
        GridTile recalcTile = newTile;
        foreach (GridTile adjTile in newTile.adjacentTiles)
            if (adjTile.goalDist < recalcTile.goalDist)
                recalcTile = adjTile;
        flowFieldGenerator.GenerateField(recalcTile, recalcTile.goalDist);
    }
    
    private GameObject[] getGoalTiles()
    {
        return GameObject.FindGameObjectsWithTag("goalTile");
    }


    private void OnEnable()
    {
        EnemyBehavior.OnEnemyReachedGoal += LoseLives;

        Player.OnTowerPlaced += recalcFlowField_NewTower;
        Player.OnTowerSold += recalcFlowField_NewTile;
    }

    private void OnDisable()
    {
        EnemyBehavior.OnEnemyReachedGoal -= LoseLives;
        
        Player.OnTowerPlaced -= recalcFlowField_NewTower;
        Player.OnTowerSold -= recalcFlowField_NewTile;
    }

}
