using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
        if(ps.IsAlive())
        {
            Debug.Log("Destroying explosion gameobject");
            //Destroy(gameObject);
        }
    }
}
