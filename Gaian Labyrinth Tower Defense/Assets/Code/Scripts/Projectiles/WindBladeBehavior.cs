using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeBehavior : TrackingBulletBehavior
{
    public float DistTilOrbit = 20f;
    public float rotationSpeed = 30f; // Adjust this value as needed
    //this might not be rotation speed
    public float rotationDuration = 2f; // Adjust this value as needed

    private bool isRotating = false;
    private float rotationTime = 0f;
    // Update is called once per frame
    protected override void Update()
    {
        Debug.Log(isRotating);
        //if the bullet is DistTilOrbit far from the target
        if (!isRotating)
        {
            // Calculate direction and distance
            Vector3 direction = target.position - transform.position;
            float distanceThisFrame = rotationSpeed * Time.deltaTime;
            
            base.Update();
            // Check if we should start rotating
            Debug.Log((transform.position - target.position).magnitude + "UUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU");
            Debug.Log("What fucking state am i: " + isRotating);
            Debug.Log("p");
            Debug.Log(distanceThisFrame + "PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP");
            if (Vector3.Distance(transform.position, target.position) < distanceThisFrame)
            {
                isRotating = true;
                Debug.Log("I just becameable to rotate: " + isRotating);
                rotationTime = 0f;
            }
        }
        else
        {
            speed = 0; 
            Debug.Log("i am rotating and i should not be moving");
            // Rotate around target
            rotationTime += Time.deltaTime;
            float rotationAngle = 360f * (rotationTime / rotationDuration);
            transform.RotateAround(target.position, Vector3.up, rotationAngle);

            // Check if rotation is complete
            if (rotationTime >= rotationDuration)
            {
                isRotating = false;
            }
        }

    }
}
