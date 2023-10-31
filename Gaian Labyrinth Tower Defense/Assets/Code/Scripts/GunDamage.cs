using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamage : MonoBehaviour
{
    public int Damage;
    public float BulletRange;
    private Transform PlayerCamera;
    
    RaycastHit hit;
    public GameObject Laser;
    
    public float FireCooldown;

    public bool Automatic;
    private float CurrentCooldown;

   
    void Start()
    {
        PlayerCamera = Camera.main.transform;
       
    }

    void Update()
    {
        if (Automatic)
        {
            //if mouse held down
            if(Input.GetMouseButton(0))
            {
                //if cd is 0 or less
                if (CurrentCooldown <= 0f)
                {
                    //go on cd again
                    Shoot();
                    CurrentCooldown = FireCooldown;
                }
            }
        }
        else
        {
            //if mouse pressed
            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentCooldown <= 0f)
                {
                    Shoot();
                    CurrentCooldown = FireCooldown;
                }
            }
        }

        //lower the cd
        CurrentCooldown -= Time.deltaTime;
    }


    // Update is called once per frame
    public void Shoot()
    {
        Ray gunRay = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
Instantiate(Laser, transform.position, transform.rotation);
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                EnemyBehavior e = hitInfo.collider.gameObject.GetComponent<EnemyBehavior>();
                e.takeDamage(Damage, gameObject);
                //hitInfo.collider.gameObject.takeDamage(10);
            }
        }
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 200, Color.yellow);
    }
}
