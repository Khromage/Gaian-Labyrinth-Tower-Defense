using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
//using static System.Net.Mime.MediaTypeNames;

public class PlayerHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text WavesText; 
    public TMP_Text TimeText;
    public TMP_Text LivesText;
    public TMP_Text CurrencyText;
    [SerializeField]
    public TMP_Text CountText;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject textboxPrefab;

    [SerializeField]
    private Image healthBar;
    private Coroutine healthBarAnimCoroutine;


    [SerializeField]
    private Image currentTowerIcon;
    
    
    [SerializeField]
    private GameObject[] weaponLayouts;

    [SerializeField]
    private GameObject[] manaPips;



    [SerializeField]
    private Image manaBar;
    private Coroutine manaBarAnimCoroutine;

    [SerializeField]
    private GameObject activeTowerPanel;

    //private int counter = 0;

    [SerializeField]
    private TowerList towerList;

    [SerializeField]
    public int[] towerSet;
    private int[] weaponSet;

    void Start()
    {
  

    }

    // Update is called once per frame
    void Update()
    {
        WavesText.text = "Wave: " + LevelManager.Instance.Wave.ToString();
        TimeText.text = "Next Wave: " + LevelManager.Instance.Countdown.ToString();
        LivesText.text = "Lives:\n" + LevelManager.Instance.Lives.ToString();
        CurrencyText.text = "$" + LevelManager.Instance.Currency.ToString();
        
        //LivesText.text = "hey " + counter;
        //counter++;
        //CountText.text = GetComponent<SpawnPoint>().waveSet[cw -1].waveEnemies.Length.ToString();
    }


    private void InitializeHUD()
    {
        towerSet = LoadoutManager.Instance.EquippedTowerIDs;
        weaponSet = LoadoutManager.Instance.EquippedWeaponIDs;

        for(int i=0; i<towerSet.Length; i++)
        {
            if(towerSet[i] != -1)
            {
                currentTowerIcon.sprite = towerList.GetTowerIcon(towerSet[i]);
                return;
            }
        }
    }

    private void FillEquipHUD()
    {
        for (int i = 0; i < towerSet.Length; i++)
        {
            activeTowerPanel.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(1).GetComponent<Image>().sprite = towerList.GetTowerIcon(towerSet[i]);
        }
        for (int i = 0; i < weaponSet.Length; i++)
        {
            //SSS fill in the active weapon slots with their respective weapon icons...
        }
    }

    private void player_updateHealthBar(float changeAmount, bool animate)
    {
        if (animate)
        {
            //Debug.Log($"starting mana bar animation, change amount = {changeAmount}");
            if (healthBarAnimCoroutine != null)
            {
                StopCoroutine(healthBarAnimCoroutine);
            }

            healthBarAnimCoroutine = StartCoroutine(animateHealthBar(changeAmount));
        }
        else
        {
            healthBar.fillAmount += changeAmount;
        }
        
        if (healthBar.fillAmount == 1)
        {
            // blue particle aura
        }
        else if (healthBar.fillAmount < .2f)
        {
            //border is more and more glowy red/crimson based on how low it is
        }
        else
        {
            //border is gray/normal
        }
    }
    private IEnumerator animateHealthBar(float delta)
    {
        float elapsedTime = 0f;
        float initialHealth = healthBar.fillAmount;

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
            if (delta < -.5f) //if spending mana, make bar purple, and transition back to blue over time
                healthBar.color = new Color(delta - elapsedTime * delta, 0f + .32f * elapsedTime, 1f, 1f);

            changeDuringThisLoop = delta * Time.deltaTime * animSpeed;
            healthBar.fillAmount += changeDuringThisLoop;
            totalChange += changeDuringThisLoop;

            //lerp directly sets fillAmount (doesn't account for changes in value midway through animation, so I'm using += instead)
            //healthBar.fillAmount = Mathf.Lerp(initialMana, initialMana + delta, elapsedTime);
            elapsedTime += Time.deltaTime * animSpeed;

            yield return null;
        }

        healthBar.color = new Color(0f, .32f, 1f, 1f);
        healthBar.fillAmount += delta - totalChange;
        //healthBar.fillAmount = initialMana + delta;
        //Debug.Log("mana should now be at " + (healthBar.fillAmount * 100f) + "%");
    }


    private void player_updateManaBar(float mana, float maxMana, bool animate)
    {
        int numFullManaPips = (int)mana / 10;
        for(int i=0; i < numFullManaPips; i++)
        {
            manaPips[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
        }
        for(int i=numFullManaPips; i < manaPips.Length; i++)
        {
            manaPips[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
        
        if(mana % 10 != 0)
            manaPips[numFullManaPips].transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = (mana % 10) / (maxMana / manaPips.Length);

        if (animate)
        {
            //Debug.Log($"starting mana bar animation, change amount = {changeAmount}");
            if (manaBarAnimCoroutine != null)
            {
                StopCoroutine(manaBarAnimCoroutine);
            }

            //manaBarAnimCoroutine = StartCoroutine(animateManaBar(changeAmount));
        }
        else
        {
            //manaBar.fillAmount += changeAmount;
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
    /*
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
            if (delta < -.5f) //if spending mana, make bar purple, and transition back to blue over time
                manaBar.color = new Color(delta - elapsedTime * delta, 0f + .32f * elapsedTime, 1f, 1f);

            changeDuringThisLoop = delta * Time.deltaTime * animSpeed;
            manaBar.fillAmount += changeDuringThisLoop;
            totalChange += changeDuringThisLoop;

            //lerp directly sets fillAmount (doesn't account for changes in value midway through animation, so I'm using += instead)
            //manaBar.fillAmount = Mathf.Lerp(initialMana, initialMana + delta, elapsedTime);
            elapsedTime += Time.deltaTime * animSpeed;

            yield return null;
        }

        manaBar.color = new Color(0f, .32f, 1f, 1f);
        manaBar.fillAmount += delta - totalChange;
        //manaBar.fillAmount = initialMana + delta;
        //Debug.Log("mana should now be at " + (manaBar.fillAmount * 100f) + "%");
    }
    */

    private void player_selectTower(int towerIndex)
    {
        if(towerIndex != -1)
        {
            currentTowerIcon.sprite = towerList.GetTowerIcon(towerSet[towerIndex]);
            Debug.Log($"Tower in slot index {towerIndex} selected");
        }

    }
    private void noTowerInSlot_Indicator(int towerIndex)
    {
        //instantiate text box
        //coroutine of it fading
        GameObject noTowerText = Instantiate(textboxPrefab, GameObject.Find("ActiveTower_Panel").transform);
        noTowerText.name = "noTowerTextbox";

        TMP_Text text = noTowerText.GetComponent<TMP_Text>();
        text.text = "No Tower assigned to this position";
        text.fontSize = 32;

        // Text position
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = noTowerText.transform.parent.GetChild(0).GetChild(towerIndex).GetComponent<RectTransform>().localPosition + new Vector3(0f, 30f, 0f); ;
        rectTransform.sizeDelta = new Vector2(400, 200);
        StartCoroutine(fadingNoTowerText(noTowerText));
    }
    private IEnumerator fadingNoTowerText(GameObject noTowerText)
    {
        RectTransform rectTransform = noTowerText.GetComponent<TMP_Text>().GetComponent<RectTransform>();
        float timeElapsed = 0f;
        while (timeElapsed < 1f)
        {
            rectTransform.localPosition = rectTransform.localPosition + new Vector3(0f, 15f * Time.deltaTime, 0f);
            noTowerText.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f - Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(noTowerText);
    }

    private void player_enterCombatMode(int weaponIndex)
    {
        //SSS remove tower highlight
        //emphasize weapon set
        //highlight/enlarge selected weapon
    }

    private void player_swapWeapon(int newWeaponIndex)
    {
        //change weapon highlight/enlargement
    }



    private void OnEnable()
    {
        Player.OnAdjustHealth += player_updateHealthBar;
        Player.OnAdjustMana += player_updateManaBar;
        Player.OnEnterCombatMode += player_enterCombatMode;
        Player.OnSwapWeapon += player_swapWeapon;
        TowerSelectionWheel.OnTowerSelected += player_selectTower;

        UIManager.OnLevelUILoaded += InitializeHUD;
        UIManager.OnLevelUILoaded += FillEquipHUD;
    }
    private void OnDisable()
    {
        Player.OnAdjustHealth -= player_updateHealthBar;
        Player.OnAdjustMana -= player_updateManaBar;
        Player.OnEnterCombatMode -= player_enterCombatMode;
        Player.OnSwapWeapon -= player_swapWeapon;
        TowerSelectionWheel.OnTowerSelected -= player_selectTower;

        UIManager.OnLevelUILoaded -= InitializeHUD;
        UIManager.OnLevelUILoaded -= FillEquipHUD;
    }
}
