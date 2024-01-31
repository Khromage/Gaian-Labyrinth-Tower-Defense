using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//wave start as an event? invoke

public class LevelManager : MonoBehaviour
{
    //Wave start event. Invoked on every new wave. SpawnPoints (and potentially other classes) will use the event.
    public delegate void WaveStart(int waveNum);
    public static event WaveStart OnWaveStart;

    public delegate void LoadData(string[] towerSet, string[] weaponSet);
    public static event LoadData OnLoadData;
    
    public PlayerInfo savedData;
    public LevelData levelData;

    //Timer
    private float waveTimer = 10f; //total time between waves
    public float waveCountdown; //currently remaining time. Display this.

    public int currWave = 0;

    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [SerializeField] private GameObject goalTile;
    public FlowFieldGenerator flowFieldGenerator;

    public Material visibleSquare;


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
        savedData = new PlayerInfo();
        LoadSavedData();

        flowFieldGenerator = new FlowFieldGenerator();
        flowFieldGenerator.visibleSquare = visibleSquare;
        flowFieldGenerator.GenerateField(goalTile.GetComponent<GridTile>(), 0);
        
        waveCountdown = 1f;
        remainingLives += 5;

        SceneManager.LoadScene("InGameHUD", LoadSceneMode.Additive);
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

        // Update levelData
        levelData.lives = remainingLives;
        levelData.wave = currWave;
        levelData.countdown = (int)waveCountdown;
    }


    public void addSpawnPoint(SpawnPoint spawnPoint)
    {
        spawnPoints.Add(spawnPoint);
        spawnPoint.OnSpawnedEnemy += enemySpawned;
    }

    private void enemySpawned(EnemyBehavior enemy)
    {
        enemy.OnEnemyReachedGoal += LoseLives;
        enemy.OnEnemyDeath += nothingRN;
    }
    private void LoseLives(EnemyBehavior enemy)
    {
        int harm = enemy.GetComponent<EnemyBehavior>().harm;
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




    public void LoadSavedData()
    {
        string jsonData = PlayerPrefs.GetString("MyProgress");
        //Convert to Class but don't create new Save Object. Re-use loadedData and overwrite old data in it
        JsonUtility.FromJsonOverwrite(jsonData, savedData);

        OnLoadData?.Invoke(savedData.ActiveTowers, savedData.ActiveWeapons);
    }
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(savedData);
        //Save Json string
        PlayerPrefs.SetString("MyProgress", jsonData);
        PlayerPrefs.Save();

        //SSS finish saving animation
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

        foreach (var s in spawnPoints)
        {
            s.OnSpawnedEnemy -= enemySpawned;
        }

        SaveData();
    }

}
