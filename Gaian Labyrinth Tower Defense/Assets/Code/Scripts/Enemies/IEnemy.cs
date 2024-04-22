using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(float amount);
    void ApplyRoot(float duration);
}
