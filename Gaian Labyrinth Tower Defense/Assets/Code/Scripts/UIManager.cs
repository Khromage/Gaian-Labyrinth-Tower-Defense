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
    private GameObject levelManager;
    [SerializeField]
    private GameObject player;

    //private int counter = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WavesText.text = "Wave: " + levelManager.GetComponent<LevelManager>().currWave.ToString();
        TimeText.text = "Time to next wave: " + ((int)levelManager.GetComponent<LevelManager>().waveCountdown).ToString();
        LivesText.text = "Lives: " + levelManager.GetComponent<LevelManager>().remainingLives.ToString();
        CurrencyText.text = "$" + player.GetComponent<Player>().currency.ToString();
        //LivesText.text = "hey " + counter;
        //counter++;
    }
}
