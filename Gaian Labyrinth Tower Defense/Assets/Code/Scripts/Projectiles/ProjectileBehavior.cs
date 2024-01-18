using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public string targeting;
    public float damage;

    public virtual void SetTarget(Transform _target)
    {
    }

}
