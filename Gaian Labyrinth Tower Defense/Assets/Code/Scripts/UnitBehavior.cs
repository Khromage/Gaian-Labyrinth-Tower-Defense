using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    public float gravityConstant = 30f;
    public Vector3 defaultGravityDir = new Vector3(0, -1, 0);


    public void UpdateGravity(Vector3 gravityDir)
    {
        float mass = GetComponent<Rigidbody>().mass;
        gameObject.GetComponent<ConstantForce>().force = gravityDir * mass * gravityConstant;

    }
    
    public void setGravityDir()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(transform.position, -transform.up, out hitInfo, 1f);
        //also ^ layermask for gravity-affecting surfaces
        if (hit)
        {
            transform.up = hitInfo.normal;
            gameObject.GetComponent<ConstantForce>().force = -hitInfo.normal * GetComponent<Rigidbody>().mass * gravityConstant;
        }
    }

}
