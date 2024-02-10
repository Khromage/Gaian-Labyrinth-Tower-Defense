using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignMenuPlayer : MonoBehaviour
{

    public Transform playerBody;
    public Rigidbody rb;

    private float moveSpeed = 5f;

    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.Find("Body");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput);

        Vector3 lateralVelocityComponent = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 verticalVelocityComponent = new Vector3(0f, rb.velocity.y, 0f);

        
        if (inputDir.magnitude > 0f)
            rb.AddForce(inputDir * 50f);



        if (lateralVelocityComponent.magnitude > moveSpeed)
        {
            Vector3 limitedVel = lateralVelocityComponent.normalized * moveSpeed;
            rb.velocity = limitedVel + verticalVelocityComponent;
            //Debug.Log("Capping movespeed");
        }

        
        if (inputDir != Vector3.zero)
        {
            playerBody.forward = Vector3.Slerp(playerBody.forward, inputDir.normalized, 5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            grounded = Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), Vector3.down, .5f);
            if (grounded)
                rb.velocity = rb.velocity + new Vector3(0f, 2f, 0f);
        }
    }

}
