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
            //Debug.Log($"Vector toward target: {vectorTowardTarget}");

            //rotate toward target
            transform.rotation = Quaternion.LookRotation(vectorTowardTarget, transform.up);
            //scale to target
            transform.localScale = new Vector3(1f, 1f, vectorTowardTarget.magnitude / 2f);
        }
    }

    //get initial start and end points
    public void SetStartAndEnd(Transform root, Transform target)
    {
        this.root = root;
        this.target = target;
    }
}
