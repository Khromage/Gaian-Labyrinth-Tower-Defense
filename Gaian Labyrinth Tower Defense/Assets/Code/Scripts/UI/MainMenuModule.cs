using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuModule : MonoBehaviour
{
   public GameObject OMenu;
   public GameObject OPanel;
   public void PlayGame () {
      // load campaign scene
      LevelManager.Instance.LoadCampaign();
   } 

   public void OpenOptions()
   {
      OPanel.SetActive(false);
      OMenu.SetActive(true);
   }

   public void OpenPanel()
   {
      OPanel.SetActive(true);
      OMenu.SetActive(false);
   }

   public void QuitGame () {
      Application.Quit();
   }
}
