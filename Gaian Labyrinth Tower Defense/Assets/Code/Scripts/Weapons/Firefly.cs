using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : Weapon
{
    public GameObject fireflyProj;

    private Quaternion launchRot;

    private int count;
    private float offset;

    public AudioSource fireflySFX1;
    public AudioSource fireflySFX2;
    public AudioSource fireflySFX3;
    public AudioSource fireflySFX4;
    public AudioSource fireflySFX5;
    public List<AudioSource> SFXList = new List<AudioSource>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        count = 0;
        offset = 0f;

        //is there a way to do this inside unity by randoming pitch or something
        fireflySFX1 = GetComponent<AudioSource>();
        fireflySFX2 = GetComponent<AudioSource>();
        fireflySFX3 = GetComponent<AudioSource>();
        fireflySFX4 = GetComponent<AudioSource>();
        fireflySFX5 = GetComponent<AudioSource>();
        SFXList.Add(fireflySFX1);
        SFXList.Add(fireflySFX2);
        SFXList.Add(fireflySFX3);
        SFXList.Add(fireflySFX4);
        SFXList.Add(fireflySFX5);

        
    }

    public override void Fire()
    {
        Debug.Log("Firing firefly");

        //i want to play a random pitched noise from a list but not working
        SFXList[Random.Range(0,4)].Play();

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
