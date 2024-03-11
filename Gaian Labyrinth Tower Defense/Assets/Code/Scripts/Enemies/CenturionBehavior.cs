using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CenturionBehavior : EnemyBehavior
{
    [SerializeField]
    private GameObject bodySegmentPrefab;

    public enum status
    {
        Head, Body, Tail
    }

    [SerializeField]
    private status currentStatus;

    [SerializeField]
    private int bodySegmentCount;

    private CenturionBehavior prevSegment;
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (currentStatus == status.Head)
        {
            Debug.Log("am a head");
            StartCoroutine(spawnBody(bodySegmentCount));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void UpdateStatus()
    {
        //if prevSegment becomes null / dies, then this segment becomes a head
        if (prevSegment == null)
        {
            GetComponent<SpringJoint>().breakForce = 0; //easy destruction of the joint

        }
    }

    private void BecomeHead()
    {
        //set active the head crest stuff
        //adjust armor/speed. maybe count how many segments are behind? Would need to know next segment as well... 
    }

    private IEnumerator spawnBody(int segmentCount)
    {
        CenturionBehavior prev = this;
        int spawned = 0;
        while (spawned < bodySegmentCount)
        {
            GameObject currB = Instantiate(bodySegmentPrefab, transform.position, transform.rotation);
            currB.GetComponent<SpringJoint>().connectedBody = prev.GetComponent<Rigidbody>();
            currB.GetComponent<CenturionBehavior>().prevSegment = prev;
            prev = currB.GetComponent<CenturionBehavior>();
            spawned++;
            yield return null; //WaitForSeconds
        }
    }
}
