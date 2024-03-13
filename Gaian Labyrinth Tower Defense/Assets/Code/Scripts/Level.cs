using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.ComponentModel;
using UnityEngine.AI;

//if (currentLevelInfo.Name == "TestScene1")   drop these once we decide on nav mesh or tiles

//wave start as an event? invoke

public class Level : MonoBehaviour
{
    //Wave start event. Invoked on every new wave. SpawnPoints (and potentially other classes) will use the event.
    public delegate void WaveStart(int waveNum);
    public static event WaveStart OnWaveStart;

    public delegate void LoadData(int[] towerSet, int[] weaponSet);
    public static event LoadData OnLoadData;
    
    public EnemyList enemyList;
    private LevelInfo currentLevelInfo;

    //Timer
    private float waveTimer; //total time between waves
    public float waveCountdown; //currently remaining time. Display this.

    public int currWave;
    [field: SerializeField]
    public GameObject[] SpawnPoints;
    private int FinishedSpawnPoints;

    [SerializeField] private GameObject goalTile;
    public FlowFieldGenerator flowFieldGenerator;

    public Material visibleSquare;

    //indexes correspond to enemy IDs. values represent how many of that type remain in wave (on wave start, totals the amount that will be spawned)
    private int[] RemainingEnemiesInWave;

    public int remainingLives;

    //gameMode? (difficulty?)

    // Start is called before the first frame update
    void Start()
    {
        currentLevelInfo = LevelManager.Instance.currentLevel;

        //if statement for using FlowFieldGenerator only in TestScene1. otherwise using NavMesh in NavTestScene
        if (currentLevelInfo.Name == "TestScene1")
        {
            flowFieldGenerator = new FlowFieldGenerator();
            flowFieldGenerator.visibleSquare = visibleSquare;
            flowFieldGenerator.GenerateField(goalTile.GetComponent<GridTile>(), 0);
        }
        
        currWave = 0;
        waveTimer = 10f;
        waveCountdown = 0f;
        FinishedSpawnPoints = 0;
        remainingLives = 25;

        SceneManager.LoadScene("UI Scene", LoadSceneMode.Additive);

        RemainingEnemiesInWave = new int[enemyList.EnemyDataSet.Length];
    }

    // Update is called once per frame
    void Update()
    {
        //Gameplay/design decision: maybe wait to start countdown until wave has been defeated, or just until they've all spawned. 

        if (FinishedSpawnPoints >= SpawnPoints.Length)
        {
            waveCountdown -= Time.deltaTime;
        }
        if (currWave < currentLevelInfo.Waves.Length && waveCountdown <= 0) //currWave < # of waves.
        {
            StartWave(currWave);
            FinishedSpawnPoints = 0;
            waveCountdown = waveTimer;
            currWave++;
            LevelManager.Instance.Wave = currWave;
        }

        // Update levelInfo
        LevelManager.Instance.Lives = remainingLives;
        LevelManager.Instance.Countdown = (int)waveCountdown;
    }

    private void StartWave(int wave)
    {
        //can either reset the RemainingEnemiesInWave array each wave, or decrement it whenever an enemy dies or reaches the goal
        System.Array.Clear(RemainingEnemiesInWave, 0 , RemainingEnemiesInWave.Length);

        Debug.Log("Starting wave (" + currWave + ")");

        for(int i = 0; i < currentLevelInfo.Waves[wave].SpawnPoints.Length; i++)
        {
            StartCoroutine(StartSpawning(1f, currentLevelInfo.Waves[wave].SpawnPoints[i].SpawnSet, SpawnPoints[i]));
            AddEnemiesToTotal(currentLevelInfo.Waves[wave].SpawnPoints[i].SpawnSet);
        }
    }

    IEnumerator StartSpawning(float defaultDelay, int[] spawnSet, GameObject spawnPoint)
    {
        for(int i = 0; i < spawnSet.Length; i++)
        {
            yield return new WaitForSeconds(defaultDelay);

            GameObject enemy = Instantiate(enemyList.GetEnemy(spawnSet[i]), spawnPoint.transform.position, spawnPoint.transform.rotation);
            
            //for nav mesh test
            if (currentLevelInfo.Name == "NavTestScene")
            {
                enemy.GetComponent<NavMeshAgent>().SetDestination(goalTile.transform.position);
            }
            Debug.Log("Enemy spawned at spawnpoint (" + spawnSet[i] + ") during wave (" + currWave + ")");

            FinishedSpawnPoints++;
        }
    }

    //adds group to total for current wave, stored in RemainingEnemiesInWave
    private void AddEnemiesToTotal(int[] enemyGroup)
    {
        for (int i = 0; i < enemyGroup.Length; i++)
        {
            RemainingEnemiesInWave[enemyGroup[i]]++;
        }
    }

    private void enemySpawned(EnemyBehavior enemy)
    {
        enemy.OnEnemyReachedGoal += LoseLives;
        enemy.OnEnemyDeath += nothingRN;
        //remainingEnemies.enemies.Add(enemy.gameObject); //removed from list in EnemyBehavior's LateUpdate()
    }

    private void LoseLives(EnemyBehavior enemy)
    {
        int harm = enemy.harm;
        Debug.Log($"Losing {harm} lives in LevelManager. Remaining lives: {remainingLives}");
        remainingLives -= harm;

        enemy.OnEnemyReachedGoal -= LoseLives;
        enemy.OnEnemyDeath -= nothingRN;
    }
    private void nothingRN(EnemyBehavior enemy)
    {
        //move currencyGain from Player to here.
        enemy.OnEnemyReachedGoal -= LoseLives;
        enemy.OnEnemyDeath -= nothingRN;
    }


    //currently does a full Field Generation. Maybe can be made more efficient
    private void recalcFlowField_NewTower(GridTile towerTile)
    {
        Debug.Log("tower placed event in level manager. recalcing field");
        if (currentLevelInfo.Name == "TestScene1")
        {
            flowFieldGenerator.GenerateField(goalTile.GetComponent<GridTile>(), 0);
        }
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



    private void OnEnable()
    {

        Player.OnTowerPlaced += recalcFlowField_NewTower;
        Player.OnTowerSold += recalcFlowField_NewTile;
    }
    private void OnDisable()
    {
        
        Player.OnTowerPlaced -= recalcFlowField_NewTower;
        Player.OnTowerSold -= recalcFlowField_NewTile;

        //SaveManager.Instance.SaveData();
    }

}
