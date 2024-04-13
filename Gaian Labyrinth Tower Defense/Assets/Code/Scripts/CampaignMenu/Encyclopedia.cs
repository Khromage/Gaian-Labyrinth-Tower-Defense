using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public class Encyclopedia : MonoBehaviour
{
    public delegate void EncyclopediaEvent();
    public static event  EncyclopediaEvent OnCloseEncyclopedia;
    // Start is called before the first frame update
     [SerializeField]
    private GameObject encyclopediaListPanel;

    [SerializeField] private LostPage[] lostPages; 
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private TMPro.TextMeshProUGUI contentText;
    [SerializeField] private Image pageImage; 
    private int currentPageIndex = 0;
    [SerializeField] private TowerData[] towers;
    [SerializeField] private EnemyData[] enemies;

    public GameObject indexView; 
    public GameObject pageView; 
void Start()
    {
        OpenIndexView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
public void OpenIndexView()
    {
        indexView.SetActive(true);
        pageView.SetActive(false);
    }

public void OpenPageView()
    {
        indexView.SetActive(false);
        pageView.SetActive(true);
    }

public void CloseEncyclopedia()
{
  OnCloseEncyclopedia?.Invoke();
}
    public void DisplayEncyclopediaInfo(int id)
    {
        Debug.Log($"Displaying Encyclopedia Info for ID: {id}");
        LostPage page = lostPages[id];
        if (page != null)
        {
            titleText.text = page.pageTitle;
            contentText.text = page.pageContent;
            pageImage.sprite = page.pageImage;

            indexView.SetActive(false);
            pageView.SetActive(true);
        }
        else
        {
            Debug.LogError("Lost page with id " + id + " not found.");
        }
    }
    public void DisplayNextPage()
{
    // Check if the next page exists; if not, wrap around or reset to 0
    if (currentPageIndex >= lostPages.Length - 1)
    {
        currentPageIndex = 0; // Reset to the first page or handle as needed
    }
    else
    {
        currentPageIndex++; // Move to the next page
    }
    
    DisplayEncyclopediaInfo(currentPageIndex);
}

public void DisplayTowerInfo(int id)
{
    if (id >= 0 && id < towers.Length)
    {
        TowerData tower = towers[id];
        if (tower != null)
        {
            titleText.text = tower.towerName;
            contentText.text = tower.description;
            pageImage.sprite = tower.towerImage;

            indexView.SetActive(false);
            pageView.SetActive(true);
        }
        else
        {
            Debug.LogError("Tower with id " + id + " not found.");
        }
    }
    else
    {
        Debug.LogError("Tower id out of range: " + id);
    }
}

public void DisplayEnemyInfo(int id)
{
    if (id >= 0 && id < enemies.Length)
    {
        EnemyData enemy = enemies[id];
        if (enemy != null)
        {
            titleText.text = enemy.enemyName;
            contentText.text = enemy.description;
            pageImage.sprite = enemy.enemyImage;

            indexView.SetActive(false);
            pageView.SetActive(true);
        }
        else
        {
            Debug.LogError("Enemy with id " + id + " not found.");
        }
    }
    else
    {
        Debug.LogError("Enemy id out of range: " + id);
    }
}


public void DisplayPreviousPage()
{
    // Check if there is a previous page; if not, wrap around or set to the last page
    if (currentPageIndex <= 0)
    {
        currentPageIndex = lostPages.Length - 1; // Move to the last page
    }
    else
    {
        currentPageIndex--; // Move to the previous page
    }
    
    DisplayEncyclopediaInfo(currentPageIndex);
}
public void OnPrevioustPageButtonClicked()
{
    DisplayPreviousPage();
}
public void OnNextPageButtonClicked()
{
    DisplayNextPage();
}
    public void OnLostPageButtonClicked(int pageId)
    {
         Debug.Log($"Lost Page Button Clicked: {pageId}");
        DisplayEncyclopediaInfo(pageId);
    }
public void OnTowerButtonClicked(int towerId)
{
    Debug.Log($"Tower Button Clicked: {towerId}");
    DisplayTowerInfo(towerId);
}
public void OnEnemyButtonClicked(int enemyId)
{
    Debug.Log($"Enemy Button Clicked: {enemyId}");
    DisplayEnemyInfo(enemyId);
}


}
