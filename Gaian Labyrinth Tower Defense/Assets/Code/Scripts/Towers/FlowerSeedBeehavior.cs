using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeedBehavior : MonoBehaviour
{
    public GameObject flowerPrefab;  // Set this dynamically when instantiating the seed.

    void Start()
    {
        // Spawn the flower after a preset delay or upon certain conditions
        Invoke(nameof(SpawnFlower), 2f);  // Delay before the flower blooms
    }

    void SpawnFlower()
    {
        Instantiate(flowerPrefab, transform.position, Quaternion.identity);  // Instantiate the flower
        Destroy(gameObject);  // Destroy the seed after spawning the flower
    }
}