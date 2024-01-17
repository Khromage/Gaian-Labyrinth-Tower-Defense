using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMarker : MonoBehaviour
{
    //public delegate void LevelInvestigated(int num);
    //public static event LevelInvestigated OnLevelInvestigate;

    [SerializeField]
    private int levelNum;

    [SerializeField]
    private Canvas panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OpenPanel()
    {
        panel.gameObject.SetActive(true);
        Debug.Log("opening panel. player hit it...");
    }
    public void ClosePanel()
    {
        panel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggering with marker");
        if (other.gameObject.tag == "Player")
        {
            OpenPanel();
            //OnLevelInvestigate?.Invoke(levelNum);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ClosePanel();
            //OnLevelInvestigate?.Invoke(levelNum);
        }
    }
}
