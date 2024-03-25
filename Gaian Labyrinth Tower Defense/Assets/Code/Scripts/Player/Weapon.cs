using UnityEngine.Events;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // [Header("Poggers")]

    // public UnityEvent OnGunShoot;

    public delegate void Fired(float manaSpent);
    public static event Fired OnFire; 

    public int Damage;
    public float ProjRange;

    public float manaCost;
    
    public float FireCooldown;

    //default is semi
    public bool Automatic;
    public float CurrentCooldown;

    public GameObject aimTarget;

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

    public void TryToFire(float currMana, Ray aimRay)
    {

        if (CurrentCooldown <= 0 && manaCost <= currMana)
        {
            GetAimTarget(aimRay);

            Fire();
            OnFire?.Invoke(-manaCost);
            CurrentCooldown = FireCooldown;
            
        }
        else if (CurrentCooldown <= 0 && manaCost > currMana)
        {
            Debug.Log("Insufficient mana to fire current weapon.");
        }

    }

    public virtual void Fire()
    {
        
    }


    //gives any enemy player camera/reticle is aiming at during fire
    public void GetAimTarget(Ray aimRay)
    {
        if ((Physics.Raycast(aimRay, out RaycastHit hit, 30f)))
        {
            if ((hit.transform.tag.Equals("Enemy")))
            {
                aimTarget = hit.transform.gameObject;
            }
        }
    }
}