using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyEnemiesArcaneTowerMission", menuName = "Mission/Destroy Enemies Arcane Tower")]
public class DestroyEnemiesArcaneTowerMission : MissionBase
{
    public int requiredKills = 20; // Set the target kills to 20
    private int currentKills = 0; // Track the number of kills

    
    public void OnEnemyKilledByArcaneTower()
    {
        currentKills++;
        CheckMissionCompletion();
    }

    public override void CheckMissionCompletion()
    {
        if (currentKills >= requiredKills && !isCompleted)
        {
            // Mark the mission as completed
            isCompleted = true;
            
            // Award extra points here
            AwardExtraPoints();

            // Notify player of mission completion, update UI, etc.
        }
    }

    private void AwardExtraPoints()
    {
        // I will implement the logic to award points to the player
   
    }
}