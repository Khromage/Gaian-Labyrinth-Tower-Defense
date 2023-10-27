using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : GridTile
{
    //might have event here for when an enemy reaches this kind of tile.

    void OnDrawGizmos()
    {
        // Draw a semitransparent blue box at the transforms position
        Gizmos.color = new Color(0f, .2f, 1f, .5f);
        Gizmos.DrawCube(transform.position - new Vector3(0f, .2f, 0f), new Vector3(1f, .2f, 1f));
    }

}
