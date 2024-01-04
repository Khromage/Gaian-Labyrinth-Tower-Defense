using UnityEngine.Events;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // [Header("Poggers")]

    // public UnityEvent OnGunShoot;

    public delegate void Fired(float manaSpent);
    public static event Fired OnFire; 

    public int Damage;
    public float BulletRange;

    public float manaCost;
    
    
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

    public void TryToFire(float currMana)
    {

        if (CurrentCooldown <= 0 && manaCost <= currMana)
        {
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
}