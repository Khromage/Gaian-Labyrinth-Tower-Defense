using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    Vector3 offset;
    Vector3 newPos;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        offset = Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = transform.position;
        newPos.x = Player.transform.position.x - offset.x;
        newPos.z = Player.transform.position.z - offset.z;
        transform.position = newPos;
    }
}
