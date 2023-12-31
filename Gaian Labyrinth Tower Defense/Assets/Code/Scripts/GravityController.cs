using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    Vector3 thisForceDir;

    void Start()
    {
        thisForceDir = -gameObject.transform.up;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("touched something");
        if (other.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>())
        {
            Debug.Log($"changing someone's direction of gravity: {thisForceDir}");

            other.gameObject.transform.parent.gameObject.GetComponent<UnitBehavior>().UpdateGravity(thisForceDir);

            if (other.gameObject.transform.parent.CompareTag("Player"))
            {
                other.gameObject.transform.parent.gameObject.GetComponent<Player>().rotateToSurface();

            }
            else if (other.gameObject.transform.parent.CompareTag("Enemy"))
            {
                //other.gameObject.transform.parent.gameObject.GetComponent<EnemyBehavior>().rotateToSurface();
            }

        }

    }
}
