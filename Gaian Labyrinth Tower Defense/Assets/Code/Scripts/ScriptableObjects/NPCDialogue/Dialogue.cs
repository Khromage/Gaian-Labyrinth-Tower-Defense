using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/Dialogue", fileName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [field: SerializeField]
    public string[] dialogueLines;
}

