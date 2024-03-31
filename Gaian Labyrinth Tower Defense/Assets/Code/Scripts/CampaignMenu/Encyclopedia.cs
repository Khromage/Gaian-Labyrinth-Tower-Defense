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
    public void OnLostPageButtonClicked(int pageId)
    {
         Debug.Log($"Lost Page Button Clicked: {pageId}");
        DisplayEncyclopediaInfo(pageId);
    }

}
