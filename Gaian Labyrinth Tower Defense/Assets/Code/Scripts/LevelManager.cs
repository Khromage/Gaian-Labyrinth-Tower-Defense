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
        flowFieldGenerator = new FlowFieldGenerator();
        flowFieldGenerator.GenerateField(goalTile.GetComponent<GridTile>(), 0);
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

    private void resetTower(GameObject tower)
    {
        Debug.Log("resetting tower. set active false and true");
        tower.SetActive(false);
        tower.SetActive(true);
    }

    private void OnEnable()
    {
        EnemyBehavior.OnEnemyReachedGoal += LoseLives;
        TowerBehavior.OnTargetingError += resetTower;
    }
    private void OnDisable()
    {
        EnemyBehavior.OnEnemyReachedGoal -= LoseLives;
        TowerBehavior.OnTargetingError -= resetTower;
    }

}
