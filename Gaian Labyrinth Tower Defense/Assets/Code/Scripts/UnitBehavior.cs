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

}
