using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBehavior : TrackingBulletBehavior
{
    [SerializeField]
    private GameObject sphereProj;

    private Vector2 lateralTarget;
    private Vector3 origPos;
    private float timer, timeCap;

    protected override void Start()
    {
        base.Start();
        damage = 4f;
        turnSpeed = 720f;
        speed = 40f;
        pierceAMT = 2;
        range = 7f;

        timer = .2f;
        origPos = transform.localPosition;
    }
    // Update is called once per frame
    protected override void Update()
    {
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
        */

        //sphereProj.transform.localPosition = Vector3.Lerp(origPos, lateralTarget, timer / timeCap);
        sphereProj.transform.localPosition = Random.insideUnitCircle * .6f;
    }
}
