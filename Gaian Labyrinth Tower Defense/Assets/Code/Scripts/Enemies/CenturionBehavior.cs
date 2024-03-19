using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

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
    private int segmentNum;

    private CenturionBehavior prevSegment;

    [SerializeField]
    private GameObject headPieces;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (currentStatus == status.Head)
        {
            segmentNum = 0;
            Debug.Log("am a head");
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(spawnBody(bodySegmentCount));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentStatus == status.Body || currentStatus == status.Tail)
        {
            UpdateStatus();
            //Debug.Log(GetComponent<Rigidbody>().GetAccumulatedForce());
        }
    }


    private void UpdateStatus()
    {
        //if prevSegment becomes null / dies, then this segment becomes a head
        if (!prevSegment)
        {
            Debug.Log("prev segment is null, making this a head");
            BecomeHead();
        }
    }

    private void BecomeHead()
    {
        currentStatus = status.Head;
        GetComponent<SpringJoint>().breakForce = 0; //easy destruction of the joint
        headPieces.SetActive(true);
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;

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
            segmentNum = spawned;
            Debug.Log($"segment {segmentNum}'s prevSegment = {currB.GetComponent<CenturionBehavior>().prevSegment}");
            Debug.Log($"segment {segmentNum}'s spring's connected body = {currB.GetComponent<SpringJoint>().connectedBody}");
            yield return new WaitForSeconds(.05f);
        }
    }
}
