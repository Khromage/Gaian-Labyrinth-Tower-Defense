using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjBehavior : TrackingBulletBehavior
{
    [SerializeField]
    private GameObject arcVisual;

    //for the dumb implementation
    [SerializeField]
    private GameObject sphereProj;
    //private Vector2 lateralTarget;
    //private Vector3 origPos;
    //private float timer, timeCap;

    private GameObject visualObj, visualObjParent;
    private Transform prevTarget;

    protected override void Start()
    {
        range = 7f;
        Destroy(gameObject, 1f);

        Vector3 vectorTowardTarget;
        prevTarget = transform;
        visualObjParent = new GameObject("Lightning bolt visuals");
        //visualObj = visualObjParent;
        Debug.Log($"pierceAMT: {pierceAMT}, before any chaining, in Start");
        for (int remChain = pierceAMT; remChain >= 0; remChain--)
        {
            //instantiate the visual
            Debug.Log($"Instantiating visual lightning bolt. {remChain} more enemies to hit");
            visualObj = Instantiate(arcVisual, transform.position, transform.rotation, visualObjParent.transform);
            Destroy(visualObj, .5f);

            //set the angle and scale the length of the visual to hit the next enemy
            vectorTowardTarget = target.position - transform.position;
            visualObj.transform.rotation = Quaternion.LookRotation(vectorTowardTarget, transform.up);
            visualObj.transform.localScale = new Vector3(1f, 1f, vectorTowardTarget.magnitude/2f);
            visualObj.GetComponent<LightningArcBehavior>().SetStartAndEnd(prevTarget, target);

            //move to and Hit target
            transform.position = target.position;
            prevTarget = target;
            HitTarget(target.gameObject);
            //if more to pierce, update target.
            if (remChain > 0)
                GetTargetInfo();
        }

        //for the dumb implementation
        //timer = .2f;
        //origPos = transform.localPosition;
    }
    // Update is called once per frame
    protected override void Update()
    {
        //rotate each visual, starting from parent
        


        //lightning arc


        /* DUMB IMPLEMENTATION. proj travels and shifts laterally a lot while leaving a trail
        base.Update();
        timer -= Time.deltaTime;

        /*
        if (timer <= 0)
        {
            timer = Random.value * .01f;
            timeCap = timer;
            origPos = new Vector3(sphereProj.transform.localPosition.x, sphereProj.transform.localPosition.y);
            lateralTarget = Random.insideUnitCircle * .5f;
            Debug.Log($"outer pos: {transform.position}");
            Debug.Log($"inner pos: {sphereProj.transform.position}");
            Debug.Log($"inner localpos: {sphereProj.transform.localPosition}");
        }
        

        //sphereProj.transform.localPosition = Vector3.Lerp(origPos, lateralTarget, timer / timeCap);
        sphereProj.transform.localPosition = Random.insideUnitCircle * .6f;
        */


    }
}
