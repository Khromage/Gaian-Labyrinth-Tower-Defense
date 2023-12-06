using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text WavesText; 
    public TMP_Text TimeText;
    public TMP_Text LivesText;
    public TMP_Text CurrencyText;
    [SerializeField]

    public TMP_Text CountText;
    [SerializeField]
    private GameObject levelManager;
     [SerializeField]
    private GameObject player;

    //private int counter = 0;

    void Start()
    {
        //get list of all spawnpoints
        //every time wave start, get all enemies

    }

    // Update is called once per frame
    void Update()
    {
        //int cw;
        //cw = levelManager.GetComponent<LevelManager>().currWave;
        WavesText.text = "Wave: " + levelManager.GetComponent<LevelManager>().currWave.ToString();
        TimeText.text = "Next Wave: " + ((int)levelManager.GetComponent<LevelManager>().waveCountdown).ToString();
        LivesText.text = "Lives:\n" + levelManager.GetComponent<LevelManager>().remainingLives.ToString();
        CurrencyText.text = "$" + player.GetComponent<Player>().currency.ToString();
        //LivesText.text = "hey " + counter;
        //counter++;
        //CountText.text = GetComponent<SpawnPoint>().waveSet[cw -1].waveEnemies.Length.ToString();
    }

    void WaveStart () {}
}
