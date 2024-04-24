using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(Techtree))]
public class TechTreeEditor : Editor
{
    //positioning
    Vector2 nodeSize = new Vector2(100f, 70f);
    float minTreeHeight = 720f;
    float minTreeWidth = 1000f;
    Vector2 incomingEdgeVec = new Vector2(100f, 10f);
    Vector2 outgoingEdgeVec = new Vector2(-12f, 10f);
    Vector2 upArrowVec = new Vector2(-10f, -10f);
    Vector2 downArrowVec = new Vector2(-10f, 10f);
    Vector2 nextLineVec = new Vector2(0f, 20f);
    Vector2 indentVec = new Vector2(102f, 0f);
    Vector2 nodeContentSize = new Vector2(40f, 20f);
    Vector2 nodeLabelSize = new Vector2(100f, 20f);

    //scrolling and move
    Vector2 mouseSelectionOffset;
    Vector2 scrollPosition = Vector2.zero;
    Vector2 scrollStartPos;

    TechNode activeNode; //moved node stored here
    TechNode selectedNode; // selected node stored here

    public override void OnInspectorGUI()
    {
        TechTree targetTree = (Techtree)target; // get the tech tree

        //mouse events
        Event currentEvent = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        EventType UIEvent = currentEvent.GetTypeForControl(controlID);

        //node styles
        GUIStyle nodeStyle = new GUIStyle(EditorStyles.helpBox);
        GUIStyle selectedStyle = new GUIStyle(EditorStyles.helpBox);
        selectedStyle.fontStyle = FontStyle.BoldAndItalic;

        //techtree view
        EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.MinHeight(720));

        for(int nodeIdx = 0; nodeIdx<targetTree.tree.Count; nodeIdx++)
        {
            //draw node
            Rect nodeRect = new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition, nodeSize);
            EditorGUI.BeginFoldoutHeaderGroup(nodeRect, true, targetTree.tree[nodeIdx].tech.name, (selectedNode == targetTree.tree[nodeIdx]? selectedStyle: nodeStyle));
            EditorGUI.LabelField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec, nodeLabelSize), "Research cost");
            targetTree.tree[nodeIdx].researchCost = EditorGUI.IntField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec + indentVec, nodeContentSize), targetTree.tree[nodeIdx].researchCost);
            EditorGUI.LabelField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec * 2, nodeLabelSize), "Invested");
        }
    }
}
