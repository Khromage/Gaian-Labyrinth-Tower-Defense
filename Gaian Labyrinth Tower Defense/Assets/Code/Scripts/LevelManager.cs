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

    private int currWave = 0;


    [SerializeField]
    private int remainingLives = 20;
    //maybe an event where an enemy reaches the goal? invoked by the enemy, then in this script adjust remainingLives

    //spawnPoint list
        //FindGameObjectWithName? SpawnPointSet, and grab all of its children for our list to use for the waves.
        //or manually fill the list with the spawn point objects back in the inspector for each level
    //gameMode? (difficulty?)

    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = 1f;
        remainingLives += 5;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currWave < 3) //currWave < # of waves.
        {
            //Gameplay/design decision: maybe wait to start countdown until wave has been defeated, or just until they've all spawned. 
            waveCountdown -= Time.deltaTime;

            if (waveCountdown <= 0)
            {
                waveCountdown = waveTimer;
                currWave++;
                OnWaveStart?.Invoke(currWave);
            }
        }
    }

}
