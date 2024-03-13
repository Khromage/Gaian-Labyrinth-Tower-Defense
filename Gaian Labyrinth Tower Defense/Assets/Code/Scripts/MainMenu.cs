using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame () {
      // load campaign scene
      LevelManager.Instance.LoadCampaign();
   } 

   public void QuitGame () {
      Application.Quit();
   }
}
