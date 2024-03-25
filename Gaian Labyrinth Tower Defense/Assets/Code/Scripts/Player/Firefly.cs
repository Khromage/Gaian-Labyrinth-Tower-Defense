using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : Weapon
{
    public GameObject fireflyProj;
    Transform FirePoint;

    private Quaternion launchRot;

    private int count;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        FirePoint = transform.Find("FirePoint");

        Damage = 1; //unused atm
        ProjRange = 9f;
        manaCost = 5;
        FireCooldown = .08f;
        Automatic = true;

        count = 0;
        offset = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentCooldown -= Time.deltaTime;
    }

    public override void Fire()
    {
        Debug.Log("Firing firefly");

        //launchRot = transform.rotation * Quaternion.Euler(Vector3.up * offset);
        launchRot = Quaternion.AngleAxis(offset, transform.eulerAngles);
        //pi/3, pi/3, pi/2 pattern

        GameObject proj = Instantiate(fireflyProj, FirePoint.transform.position, launchRot);
        //maybe set proj.layer here, to a layer that ignores the weapon hitbox
        Vector3 randPos = FirePoint.transform.position + (FirePoint.transform.up * (1 + Random.value))
            + FirePoint.transform.right * (Random.value * 2f - 1f) + FirePoint.transform.forward * Random.value * 3f;
        GameObject t = new GameObject();
        t.transform.position = randPos; 
        //Destroy(t, 1f);

        proj.GetComponent<FireflyProj>().SetTarget(t.transform);
        proj.GetComponent<FireflyProj>().enemyTarget = aimTarget;

        count++;
        if (count % 3 == 0)
        {
            offset += 180;
        }
        else
        {
            offset += 120;
        }

        if (count == 12) //maybe switch to 18 for consistency
            count = 0;
    }
}
