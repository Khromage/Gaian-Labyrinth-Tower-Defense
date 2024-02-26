using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenturionBehavior : EnemyBehavior
{

    public enum status
    {
        Head, Body, Tail
    }

    [SerializeField]
    private status currentStatus;
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void UpdateStatus()
    {

    }
}
