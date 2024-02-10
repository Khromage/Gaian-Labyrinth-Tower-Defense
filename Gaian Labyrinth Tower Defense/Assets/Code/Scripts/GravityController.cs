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
        if (other.gameObject.CompareTag("gravCore"))
        {
            other.gameObject.transform.parent.gameObject.GetComponent<UnitBehavior>().UpdateGravity(thisForceDir, transform);
            //maybe Player overrides UpdateGravity, and in its version it also starts the rotation coroutine
        }

        /*
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
        */

    }

    //is this going to break everything?
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("gravCore"))
        {
            if (other.gameObject.transform.parent.gameObject.GetComponent<UnitBehavior>().currGravDir == thisForceDir.normalized)
            {
                other.gameObject.SetActive(false);
                other.gameObject.SetActive(true);
            }
        }
    }
}
