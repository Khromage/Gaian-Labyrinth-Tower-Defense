using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(menuName = "GLTD/Tech", fileName = "Tech")]
public class Tech : ScriptableObject
{
    public string definition;
    public bool invested;
    public bool locked;
    //public Texture2D image;

    void Start()
    {
        invested = true;
        //locked = true;
    }
    void Update()
    {
        invested = true;
    }
}
