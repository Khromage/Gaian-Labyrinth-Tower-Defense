using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField]
    private Image manaBar;
    private Coroutine manaBarAnimCoroutine;

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


    private void player_updateManaBar(float changeAmount, bool animate)
    {
        if (animate)
        {
            //Debug.Log($"starting mana bar animation, change amount = {changeAmount}");
            if (manaBarAnimCoroutine != null)
            {
                StopCoroutine(manaBarAnimCoroutine);
            }

            manaBarAnimCoroutine = StartCoroutine(animateManaBar(changeAmount));
        }
        else
        {
            manaBar.fillAmount += changeAmount;
        }
        
        if (manaBar.fillAmount == 1)
        {
            //border is glowy pale blue-green
        }
        else if (manaBar.fillAmount < .2f)
        {
            //border is more and more glowy red based on how low it is
        }
        else
        {
            //border is gray/normal
        }
    }
    private IEnumerator animateManaBar(float delta)
    {
        float elapsedTime = 0f;
        float initialMana = manaBar.fillAmount;

        float totalChange = 0f;
        float changeDuringThisLoop = 0f;

        //if delta > 1/10th of total bar, then do that whole diff in 1/8th of a second
        float animSpeed = 8f;
        //otherwise do it at rate of delta in 1/delta of a second
        if (Mathf.Abs(delta) < .1f)
        {
            animSpeed = 1f / Mathf.Abs(delta);
        }

        while (elapsedTime < 1)
        {
            if (delta < 0) //if spending mana, make bar purple, and transition back to blue over time
                manaBar.color = new Color(delta - elapsedTime * delta, 0f + .32f * elapsedTime, 1f, 1f);

            changeDuringThisLoop = delta * Time.deltaTime * animSpeed;
            manaBar.fillAmount += changeDuringThisLoop;
            totalChange += changeDuringThisLoop;

            //lerp to directly set fillAmount (doesn't account for changes in value midway through animation, so I'm using += instead)
            //manaBar.fillAmount = Mathf.Lerp(initialMana, initialMana + delta, elapsedTime);
            elapsedTime += Time.deltaTime * animSpeed;

            yield return null;
        }

        manaBar.color = new Color(0f, .32f, 1f, 1f);
        manaBar.fillAmount += delta - totalChange;
        //manaBar.fillAmount = initialMana + delta;
        //Debug.Log("mana should now be at " + (manaBar.fillAmount * 100f) + "%");
    }


    private void OnEnable()
    {
        Player.OnAdjustMana += player_updateManaBar;
    }
    private void OnDisable()
    {
        Player.OnAdjustMana -= player_updateManaBar;
    }
}
