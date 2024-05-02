using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElkBehavior : EnemyBehavior
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyBehavior>().AddDamageModifier(0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyBehavior>().exitBuffZone();
        }
    }
}
