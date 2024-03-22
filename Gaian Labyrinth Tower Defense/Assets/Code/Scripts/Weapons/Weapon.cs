using UnityEngine.Events;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // [Header("Poggers")]

    // public UnityEvent OnGunShoot;

    public delegate void Fired(float manaSpent);
    public static event Fired OnFire;

    public float Damage { get; protected set; }
    public float ProjRange { get; protected set; }

    public float manaCost { get; protected set; }

    public float FireCooldown { get; protected set; }

    //default is semi
    public bool Automatic { get; protected set; }
    public float CurrentCooldown { get; protected set; }

    public Transform FirePoint { get; protected set; }
    public GameObject aimTarget;

    [SerializeField]
    private WeaponInfo weaponInfo;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //weapon on CD on equip
        CurrentCooldown = FireCooldown;

        Damage = weaponInfo.Damage;
        FireCooldown = weaponInfo.FireCooldown;
        manaCost = weaponInfo.ManaCost;
        Automatic = weaponInfo.Automatic;
        ProjRange = weaponInfo.ProjectileRange;

        FirePoint = transform.Find("FirePoint");
    }

    // Update is called once per frame
    void Update()
    {
        CurrentCooldown -= Time.deltaTime;
    }

    public bool TryToFire(float currMana, Ray aimRay)
    {
        bool success = false;
        FirePoint.rotation = Quaternion.LookRotation(aimRay.direction, transform.up);
        if (CurrentCooldown <= 0 && manaCost <= currMana)
        {
            GetAimTarget(aimRay);

            Fire();
            OnFire?.Invoke(-manaCost);
            CurrentCooldown = FireCooldown;
            success = true;
            
        }
        else if (CurrentCooldown <= 0 && manaCost > currMana)
        {
            Debug.Log("Insufficient mana to fire current weapon.");
        }

        return success;
    }

    //Instantiate projectile, specific to each weapon (hence virtual -> override)
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