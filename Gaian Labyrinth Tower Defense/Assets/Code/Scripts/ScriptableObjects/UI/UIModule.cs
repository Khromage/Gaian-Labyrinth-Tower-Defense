using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/UIModule", fileName = "UIModule")]
public class UIModule : ScriptableObject
{
    [field: SerializeField]
    public GameObject[] UIElements { get; private set; }

}