using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : SpawnableSingleton<LevelManager>
{
    public delegate void SceneManagementEvent(int ID);
    public event SceneManagementEvent OnSceneLoaded;



    public LevelInfo currentLevel;
    public int Lives, Wave, Currency;
    public float WaveTime;

    public void LoadLevel(LevelInfo level)
    {
        currentLevel = level;
        
        if(SceneManager.GetSceneByName("campaignMenu") != null)
        {
            SceneManager.UnloadSceneAsync("campaignMenu");
        }

        //Debug.Log($"About to load: {currentLevel.Name}");
        
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
        //Debug.Log($"in LoadingScene coroutine: {currentLevel.Name}");

        while(!scene.isLoaded)
        {
            Debug.Log("SCENE LOADING");
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        
        // CHANGE INT TO INDEX OF SCENE IN DATALIST SO
        // Also lets PlayerHUD know HUD is ready to be initi
        OnSceneLoaded?.Invoke(ID);

    }


    private void UpdateWaveInfo(int currentWave) {
        Wave = currentWave;
        
        // RESETS TIMER UPON WAVE START
        //WaveTime = 0f;

    }

    void Update() {
        WaveTime += Time.deltaTime;
    }

    void OnEnable() {
        //Debug.Log("IM ENABLED");
        Level.OnWaveStart += UpdateWaveInfo;

    }

    void OnDisable() {
        //Debug.Log("IM DISABLED");
        Level.OnWaveStart -= UpdateWaveInfo;

    }



}
