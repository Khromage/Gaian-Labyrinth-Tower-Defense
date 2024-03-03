using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraMain : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject playerObj;
    void Update()
    {
        playerCam.transform.position = playerObj.transform.position;
    }
}
