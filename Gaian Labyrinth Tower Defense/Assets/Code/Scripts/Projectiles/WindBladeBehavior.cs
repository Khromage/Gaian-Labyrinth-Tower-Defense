using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeBehavior : TrackingBulletBehavior
{
    public float DistTilOrbit = 20f;
    public float rotationSpeed = 30f; // Adjust this value as needed
    //this might not be rotation speed
    public float rotationDuration = 50f; // Adjust this value as needed
    private int numRotations;
    public int desiredRotations;
    float distanceThisFrame = 10f;
    private bool isRotating = false;
    private float rotationTime = 0f;
    // Update is called once per frame

    protected override void Start()
    {
        base.Start();
        numRotations = 0;
        desiredRotations = 1;
    }

    protected override void Update()
    {
        Debug.Log(isRotating);
        //if the bullet is DistTilOrbit far from the target
        if (!isRotating && (target != null) )
        {
            // Calculate direction and distance
            Vector3 direction = target.position - transform.position;

            
            base.Update();
            // Check if we should start rotating
           
            if ( (Vector3.Distance(transform.position, target.position) < distanceThisFrame) && (numRotations < desiredRotations) )
            {
                //transform.Rotate(45, 45, 0); causese the blade to look diagonally down to the target, maybe this could solve the colliding into non target enemies issue by raising the blade up somehow
                transform.Rotate(0, 90, 0);
                isRotating = true;
                Debug.Log("I just becameable to rotate: " + isRotating);
                rotationTime = 0f;
                
            }
        }
        else
        {

            if (target == null)
            {
                //maybe go towards another target?
                Destroy(gameObject);         
                return;
            }

            Debug.Log("i am rotating and i should not be moving");
            // Rotate around target
            rotationTime += Time.deltaTime;
            // float rotationAngle = 2f * (rotationTime / rotationDuration);
            transform.RotateAround(target.position, target.up, 10f * Time.deltaTime);//* (1+Time.deltaTime));


            // Check if rotation is complete
            if (rotationTime >= rotationDuration)
            {
                numRotations++;
                rotationTime = 0f;
                Debug.Log(numRotations);
                if(numRotations >= desiredRotations) 
                {
                    isRotating = false;
                    Debug.Log("exiting rotate loop");
                }
            }


        }

    }
}
