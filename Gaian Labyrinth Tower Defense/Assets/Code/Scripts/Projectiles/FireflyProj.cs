using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyProj : TrackingBulletBehavior
{
    public GameObject enemyTarget;


    // Start is called before the first frame update
    protected override void Start()
    {
        targeting = "Close";

        speed = 8f;
        damage = 1f;
        pierceAMT = 0;
        range = 8f; // should match firefly weapon probably

        StartCoroutine(speedAdjustments());
    }

    // Update is called once per frame
    protected override void Update()
    {
        //if target.tag != enemy && dist from target > .5, slow down until hover, then once hovering, retarget to enemy
        if (target != null)
        {
            MoveTowardTarget();
        }
        else
        {
            if (enemyTarget == null)
            {
                Debug.Log("Retargeting");
                GetTargetInfo();
            }
            else
            {
                target = enemyTarget.transform;
            }

            if (target == null)
                Destroy(gameObject);
        }

    }

    private IEnumerator speedAdjustments()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            if (elapsedTime < 1f)
            {
                //slow
                //speed -= Mathf.Lerp(0f, speed / 2, elapsedTime);
                speed -= 6f * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                //Debug.Log("target is: " + target.gameObject + " at " + target.position);
                yield return null;
            }/*
            else if (elapsedTime < 1.5f)
            {
                Debug.Log("1.2s have passed");
                //hover
                speed = .3f;
                elapsedTime += Time.deltaTime;
                yield return null;
            }*/
            else
            {
                if (target != null && target.gameObject.tag != "Enemy")
                {
                    Debug.Log("destroying" + target.gameObject);
                    Destroy(target.gameObject);
                    Debug.Log("destroyed non-enemy target");
                }
                speed = Mathf.Lerp(speed, 12f, (elapsedTime - 1f));
                turnSpeed += Time.deltaTime * 3f;
                elapsedTime += Time.deltaTime;
                //accelerate back to speed
                yield return null;
            }
        }
        speed = 12f;
        turnSpeed = 8f;

    }
}
