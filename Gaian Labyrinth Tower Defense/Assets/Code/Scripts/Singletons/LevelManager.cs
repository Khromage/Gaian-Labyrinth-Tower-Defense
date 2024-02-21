using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : SpawnableSingleton<LevelManager>
{
    public delegate void LevelManagementEvent(LevelInfo level);
    public event LevelManagementEvent OnLevelLoaded;
    public event LevelManagementEvent OnLevelUnloaded;

    public LevelInfo currentLevel;
    public int Lives, Wave, Countdown, Currency;

    public void LoadLevel(LevelInfo level)
    {
        currentLevel = level;
        Debug.Log(currentLevel.Name + " LEVEL SET VIA LEVELMANAGER");
        Debug.Log("LOADING " + currentLevel.Name + " SCENE");
        SceneManager.LoadScene(currentLevel.Name);
        // Load level passed in
        // Could be scene changing using LevelInfo. its all how you decide to do it
        
        // Spawn Player
        // Start Level Laoded (Waves etc)
        
        OnLevelLoaded?.Invoke(level);
    }
}
