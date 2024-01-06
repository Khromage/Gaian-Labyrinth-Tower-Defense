using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignMenuPlayer : MonoBehaviour
{

    public Transform playerBody;
    public Rigidbody rb;

    public float moveSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.Find("Body");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput);

        Vector3 lateralVelocityComponent = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 verticalVelocityComponent = new Vector3(0f, rb.velocity.y, 0f);

        
        if (inputDir.magnitude > 0f)
            rb.AddForce(inputDir * 40f);



        if (lateralVelocityComponent.magnitude > moveSpeed)
        {
            Vector3 limitedVel = lateralVelocityComponent.normalized * moveSpeed;
            rb.velocity = limitedVel + verticalVelocityComponent;
        }

        
        if (inputDir != Vector3.zero)
        {
            playerBody.forward = Vector3.Slerp(playerBody.forward, inputDir.normalized, 5f * Time.deltaTime);
        }
    }
}
