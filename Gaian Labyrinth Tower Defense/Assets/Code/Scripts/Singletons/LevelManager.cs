using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : SpawnableSingleton<LevelManager>
{
    public delegate void SceneManagementEvent(int ID);
    public event SceneManagementEvent OnSceneLoaded;



    public LevelInfo currentLevel;
    public int Lives, Wave, Countdown, Currency;

    public void LoadLevel(LevelInfo level)
    {
        currentLevel = level;
        
        if(SceneManager.GetSceneByName("campaignMenu") != null)
        {
            SceneManager.UnloadSceneAsync("campaignMenu");
        }
        
        // StartCoroutine
        SceneManager.LoadSceneAsync(currentLevel.Name, LoadSceneMode.Additive);
        
        // CHANGE INT TO INDEX OF SCENE IN DATALIST SO
        StartCoroutine(LoadingScene(SceneManager.GetSceneByName(currentLevel.Name), 2));


    }

    public void LoadCampaign()
    {
        if(currentLevel != null)
        {
            SceneManager.UnloadSceneAsync(currentLevel.Name);
            currentLevel = null;
        }

        SceneManager.LoadSceneAsync("campaignMenu", LoadSceneMode.Additive);

        // CHANGE INT TO INDEX OF SCENE IN DATALIST SO
        StartCoroutine(LoadingScene(SceneManager.GetSceneByName("campaignMenu"), 1));

    }

    IEnumerator LoadingScene(Scene scene, int ID)
    {
        while(!scene.isLoaded)
        {
            Debug.Log("SCENE LOADING");
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        
        // CHANGE INT TO INDEX OF SCENE IN DATALIST SO
        OnSceneLoaded?.Invoke(ID);

    }



}
