using UnityEngine.Events;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   // [Header("Poggers")]

   // public UnityEvent OnGunShoot;

    public int Damage;
    public float BulletRange;

    
    
    
    public float FireCooldown;

    //default is semi
    public bool Automatic;
    public float CurrentCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        //not neccessary but puts gun on cd on game start
        CurrentCooldown = FireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void TryToFire()
    {

        if (CurrentCooldown <= 0)
        {
            Fire();
            CurrentCooldown = FireCooldown;
            
        }

    }

    public virtual void Fire()
    {
        
    }
}