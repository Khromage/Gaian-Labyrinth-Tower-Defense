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

    [SerializeField] private LostPage[] lostPages; // Assign this in the Inspector
    [SerializeField] private Text titleText; // Assign in the Inspector
    [SerializeField] private Text contentText; // Assign in the Inspector
    [SerializeField] private Image pageImage; // Assign in the Inspector

    public GameObject indexView; // Assign this with your Index View container in the inspector.
    public GameObject pageView; // Assign this with your Page View container in the inspector.
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
        DisplayEncyclopediaInfo(pageId);
    }

}
