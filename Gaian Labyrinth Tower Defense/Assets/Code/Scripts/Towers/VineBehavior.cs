using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBehavior : MonoBehaviour
{
    public float damage;
    public float rootDuration;

    void Start()
    {
        // You might want to initialize some visual effects or movement here
    }

void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.ApplyRoot(rootDuration);
            Destroy(gameObject);  // Destroy the vine after applying effects
        }
    }
}