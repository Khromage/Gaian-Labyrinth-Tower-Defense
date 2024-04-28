using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureTowerBehavior : TowerBehavior
{
    public float lv1_duration;
    public GameObject vinePrefab;
    public GameObject flowerSeedPrefab;
    public List<GameObject> flowerTypes = new List<GameObject>();
    private float rootDuration;
     public GameObject enhancedFlowerPrefab;  // A prefab for more powerful flowers
    public GameObject animalDogpilePrefab;  // A prefab for the animal dogpile
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject currentTarget;


    public override void Start()
    {
        base.Start();
        rootDuration = lv1_duration;  // Assuming similar structure for duration or effect intensities as in your examples.
        StartCoroutine(TargetAndAttackRoutine());
    }
    public override void Update()
    {
        base.Update();
        SelectClosestTarget(); // Select the closest enemy as the current target
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
    private IEnumerator TargetAndAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / fireRate); // Cooldown based on the fire rate
            if (currentTarget != null)
            {
                lv1_Attack();
            }
        }
    }

    private void SelectClosestTarget()
    {
        float closestDistanceSqr = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemiesInRange)
        {
            Vector3 directionToTarget = enemy.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestEnemy = enemy;
            }
        }

        currentTarget = nearestEnemy; // This will be null if no enemies are in range
    }

    protected override void lv1_Attack()
    {
        // Lashing Vines
        GameObject vine = Instantiate(vinePrefab, firePoint.position, firePoint.rotation);
        vine.GetComponent<VineBehavior>().damage = damage;
        vine.GetComponent<VineBehavior>().rootDuration = rootDuration;

        // Flower Shoot
       GameObject seed = Instantiate(flowerSeedPrefab, firePoint.position, Quaternion.identity);
        int flowerTypeIndex = Random.Range(0, flowerTypes.Count);
        seed.GetComponent<FlowerSeedBehavior>().flowerPrefab = flowerTypes[flowerTypeIndex];
    }

protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        rootDuration *= 1.5f;  // Enhanced root duration
        // Optionally enhance flower effects here if needed
    }

    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        // Switch to enhanced flower shoot
        flowerSeedPrefab = enhancedFlowerPrefab;
    }

    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        // Animal Dogpile attack
        projectilePrefab = animalDogpilePrefab;
    }

    // Example of handling different attack types for level 3
    protected override void lv3_1_Attack()
    {
        // Enhanced Flower Shoot
        lv1_Attack();  // Uses the enhanced flower seed prefab
    }

    protected override void lv3_2_Attack()
    {
        // Animal Dogpile
        Instantiate(animalDogpilePrefab, firePoint.position, firePoint.rotation);
    }

    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        // Additional modification for Level 3.3, such as multiple seed shooting
        flowerSeedPrefab = enhancedFlowerPrefab;
        // Imagine handling multiple seed shoots or more complex flower effects
    }

    protected override void lv3_3_Attack()
    {
        // Shoot multiple seeds or more potent effects
        for (int i = 0; i < 3; i++)  // Example of shooting multiple seeds
        {
            lv1_Attack();
        }
    }
}
