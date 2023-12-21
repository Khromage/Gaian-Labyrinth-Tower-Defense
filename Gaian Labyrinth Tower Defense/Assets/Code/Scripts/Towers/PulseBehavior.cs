using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBehavior : MonoBehaviour
{
    public float maxDiameter;
    public float currentDiameter;
    public float maxDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentDiameter = .5f;
        maxDiameter = transform.parent.GetComponent<PulseTowerBehavior>().range;
        maxDamage = transform.parent.GetComponent<PulseTowerBehavior>().maxDamage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyBehavior e = other.GetComponent<EnemyBehavior>();
            e.takeDamage((maxDiameter / currentDiameter) * maxDamage, gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        currentDiameter += Time.deltaTime * maxDiameter;
        transform.localScale = new Vector3(1, 1, 1) * currentDiameter;
        if (currentDiameter >= maxDiameter)
        {
            Destroy(gameObject);
        }
    }
}
