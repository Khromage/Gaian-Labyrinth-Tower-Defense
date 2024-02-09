using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


//wave start as an event? invoke

public class Level : MonoBehaviour
{
    //Wave start event. Invoked on every new wave. SpawnPoints (and potentially other classes) will use the event.
    public delegate void WaveStart(int waveNum);
    public static event WaveStart OnWaveStart;

    public delegate void LoadData(string[] towerSet, string[] weaponSet);
    public static event LoadData OnLoadData;
    
    public PlayerInfo savedData;
    private LevelInfo currentLevelInfo;

    //Timer
    private float waveTimer; //total time between waves
    public float waveCountdown; //currently remaining time. Display this.

    public int currWave;
    [field: SerializeField]
    public GameObject[] SpawnLocations;

    public List<EnemyBehavior> enemyList = new List<EnemyBehavior>();

    [SerializeField] private GameObject goalTile;
    public FlowFieldGenerator flowFieldGenerator;

    public Material visibleSquare;

    [SerializeField]
    private EnemiesRemaining remainingEnemies;

    public int remainingLives;

    //gameMode? (difficulty?)

    // Start is called before the first frame update
    void Start()
    {
        savedData = new PlayerInfo();
        currentLevelInfo = LevelManager.Instance.currentLevel;
        LoadSavedData();

        flowFieldGenerator = new FlowFieldGenerator();
        flowFieldGenerator.visibleSquare = visibleSquare;
        flowFieldGenerator.GenerateField(goalTile.GetComponent<GridTile>(), 0);
        
        currWave = 0;
        waveCountdown = 1f;
        waveTimer = 10f;
        remainingLives = 25;

        SceneManager.LoadScene("InGameHUD", LoadSceneMode.Additive);

        remainingEnemies.enemies.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (currWave < currentLevelInfo.Waves.Length) //currWave < # of waves.
        {
            //Gameplay/design decision: maybe wait to start countdown until wave has been defeated, or just until they've all spawned. 
            waveCountdown -= Time.deltaTime;

            if (waveCountdown <= 0)
            {
                waveCountdown = waveTimer;
                currWave++;
                waveTimer++;
                StartWave(currWave); // LAST THING CHANGED (KHROM)
            }
        }

        // Update levelData
        LevelManager.Instance.Lives = remainingLives;
        LevelManager.Instance.Wave = currWave;
        LevelManager.Instance.Countdown = (int)waveCountdown;
    }

    private void StartWave(int wave)
    {
        Debug.Log("Starting wave (" + wave + ")");
        Parallel.ForEach(currentLevelInfo.Waves[wave].SpawnPoints, SpawnPoint =>
        {
            StartCoroutine(StartSpawning(1f, SpawnPoint.SpawnSet));
        });
    }

    IEnumerator StartSpawning(float defaultDelay, int[] spawnSet)
    {
        for(int i = 0; i < spawnSet.Length; i++)
        {
            yield return new WaitForSeconds(defaultDelay);

            /*
            GameObject spawnedEnemy = Instantiate(waveSet[waveNum - 1].waveEnemies[i], transform.position, transform.rotation);
            EnemyBehavior spawnedEnemyScript = spawnedEnemy.GetComponent<EnemyBehavior>();
            spawnedEnemyScript.currTile = this.GetComponent<GridTile>();
            */
            Debug.Log("Enemy with ID (" + spawnSet[i] + ") was spawned");
        }
    }

    private void enemySpawned(EnemyBehavior enemy)
    {
        enemy.OnEnemyReachedGoal += LoseLives;
        enemy.OnEnemyDeath += nothingRN;
        remainingEnemies.enemies.Add(enemy.gameObject); //removed from list in EnemyBehavior's LateUpdate()
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

        SaveData();
    }

}
