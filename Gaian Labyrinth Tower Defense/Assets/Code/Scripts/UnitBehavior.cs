using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitBehavior : MonoBehaviour
{
    public float gravityConstant = 30f;
    public Vector3 defaultGravityDir = new Vector3(0, -1, 0);

    public Vector3 lateralVelocityComponent;
    public Vector3 verticalVelocityComponent;

    public Rigidbody rb;

    public Vector3 currGravDir;

    public void UpdateGravity(Vector3 gravityDir, Transform gravSource)
    {
        Debug.Log("updating gravity");
        float mass = GetComponent<Rigidbody>().mass;
        gameObject.GetComponent<ConstantForce>().force = gravityDir * mass * gravityConstant;
        currGravDir = Vector3.Normalize(gravityDir);

        //transform.up = -currGravDir;
        beginRotation(gravSource);
    }
    protected virtual void beginRotation(Transform gravSource)
    {
        //to be overridden and start coroutines in Player and maybe EnemyBehavior
    }
    
    //raycast form of determining gravity direction. defunct/unused
    /*
    public virtual void setGravityDir()
    {
        Vector3 movementOffset = lateralVelocityComponent.normalized * .3f; //or Vector3.zero;

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(transform.position + movementOffset, -transform.up, out hitInfo, 1f, 1 << 6); //1 << 6 bitshifted to layer 6, whatIsGround
        //also ^ layermask for gravity-affecting surfaces
        if (hit)
        {
            transform.up = hitInfo.normal;
            gameObject.GetComponent<ConstantForce>().force = -hitInfo.normal * GetComponent<Rigidbody>().mass * gravityConstant;
        }
    }
    */

    public void setVelocityComponents()
    {
        verticalVelocityComponent = Vector3.Normalize(GetComponent<ConstantForce>().force) * Vector3.Dot(rb.velocity, Vector3.Normalize(GetComponent<ConstantForce>().force));
        lateralVelocityComponent = rb.velocity - verticalVelocityComponent;
    }

}
