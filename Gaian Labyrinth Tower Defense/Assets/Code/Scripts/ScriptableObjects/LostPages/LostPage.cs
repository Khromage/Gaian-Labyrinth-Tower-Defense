using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLostPage", menuName = "Pages/Lost Page", order = 0)]
public class LostPage : ScriptableObject
{
    public int id;
    public string pageTitle;
    [TextArea(10, 20)]
    public string pageContent;
    public Sprite pageImage; // visuals for later.
}