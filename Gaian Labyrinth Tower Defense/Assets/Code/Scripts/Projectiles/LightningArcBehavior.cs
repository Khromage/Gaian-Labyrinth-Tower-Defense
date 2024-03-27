using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArcBehavior : MonoBehaviour
{
    private Transform root, target;
    Vector3 vectorTowardTarget;

    // Update is called once per frame
    void Update()
    {
        //if (root exists, shift my position to match the root.
        //if the target exists, rotate towards it.
        //if either don't exist, destroy me after .1s maybe? (shoulnd't be instant in case this killed the enemy. still need a visual to exist at some point)
        FitTransform();
    }

    private void FitTransform()
    {
        if (root == null || target == null)
        {
            Destroy(gameObject, .1f);
        }
        else
        {
            //shift my position
            transform.position = root.position;

            vectorTowardTarget = target.position - transform.position;
            Debug.Log($"Vector toward target: {vectorTowardTarget}");
            //rotate toward target
            transform.rotation = Quaternion.LookRotation(vectorTowardTarget, transform.up);
            //scale to target
            transform.localScale = new Vector3(1f, 1f, vectorTowardTarget.magnitude / 2f);
        }
    }

    public void SetStartAndEnd(Transform root, Transform target)
    {
        this.root = root;
        this.target = target;
    }
}
