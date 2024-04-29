using UnityEngine.Events;
using UnityEngine;
using System;

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
    public Vector3 aimHitPos;

    public WeaponInfo weaponInfo;

    public Tech dmgincrease;

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
        if (CurrentCooldown <= 0 && manaCost <= currMana)
        {
            if (GetAimTarget(aimRay))
            {
                FirePoint.rotation = Quaternion.LookRotation((aimHitPos - FirePoint.position), transform.up);
            }
            else
            {
                FirePoint.rotation = Quaternion.LookRotation(aimRay.direction, transform.up);
            }

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
    public bool GetAimTarget(Ray aimRay)
    {
        if ((Physics.Raycast(aimRay, out RaycastHit hit, 30f)))
        {
            if ((hit.transform.tag.Equals("Enemy")))
                aimTarget = hit.transform.gameObject;

            //don't aim too far inward
            if (Vector3.SqrMagnitude(hit.point - FirePoint.position) > 1)
                aimHitPos = hit.point;

            return true;
        }
        else
        {
            return false;
        }
    }

    public void DmgModifiers()
    {
        Debug.Log("dmgmod called");
        if (dmgincrease.invested == true)
        {
            Damage += 100;
            Debug.Log("success dmg increase");
        }
    }
}