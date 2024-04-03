using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encyclopedia Entry", menuName = "Encyclopedia/Entry")]
public class EncyclopediaEntry : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public Sprite image;
}
