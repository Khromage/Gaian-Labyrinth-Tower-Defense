using UnityEngine.Events;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Poggers")]

    public UnityEvent OnGunShoot;
    public float FireCooldown;

    //default is semi
    public bool Automatic;

    private float CurrentCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        //not neccessary but puts gun on cd on game start
        CurrentCooldown = FireCooldown;
    }

    // Update is called once per frame
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
                    OnGunShoot?.Invoke();
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
                    OnGunShoot?.Invoke();
                    CurrentCooldown = FireCooldown;
                }
            }
        }

        //lower the cd
        CurrentCooldown -= Time.deltaTime;
    }
}