using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Encyclopedia/Tower")]
public class TowerData : ScriptableObject {
    public string towerName;
    [TextArea(10, 20)]
    public string description;
    public Sprite towerImage;
    // Add other tower-specific properties here
}

[CreateAssetMenu(fileName = "New Enemy", menuName = "Encyclopedia/Enemy")]
public class EnemyData : ScriptableObject {
    public string enemyName;
    [TextArea(10, 20)]
    public string description;
    public Sprite enemyImage;
    // Add other enemy-specific properties here
}
