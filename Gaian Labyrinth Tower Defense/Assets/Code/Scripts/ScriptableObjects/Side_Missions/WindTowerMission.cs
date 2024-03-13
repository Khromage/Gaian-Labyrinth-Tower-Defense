using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockBackEnemiesWindTowerMission", menuName = V)]
public class KnockBackEnemiesWindTowerMission : MissionBase
{
    private const string V = "Mission/Knock Back Enemies Wind Tower";
    public int requiredKnockBacks = 25;
    private int currentKnockBacks = 0;

    // Method to be called when an enemy is knocked back by the Wind Tower
    public void OnEnemyKnockedBackByWindTower()
    {
        currentKnockBacks++;
        CheckMissionCompletion();
    }

    public override void CheckMissionCompletion()
    {
        if (currentKnockBacks >= requiredKnockBacks && !isCompleted)
        {
            isCompleted = true;
            // Here I will add any logic that should happen once the mission is completed
            
            Debug.Log("Mission Completed: Knocked back 25 enemies with the Wind Tower!");
        }
    }
}
