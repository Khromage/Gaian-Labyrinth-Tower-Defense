using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
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
        Techtree targetTree = (Techtree)target; // get the tech tree

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

        for(int nodeIdx = 0; nodeIdx < targetTree.tree.Count; nodeIdx++)
        {
            //draw node
            Rect nodeRect = new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition, nodeSize);
            EditorGUI.BeginFoldoutHeaderGroup(nodeRect, true, targetTree.tree[nodeIdx].tech.name, (selectedNode == targetTree.tree[nodeIdx]? selectedStyle: nodeStyle));
            EditorGUI.LabelField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec, nodeLabelSize), "Research cost");
            targetTree.tree[nodeIdx].researchCost = EditorGUI.IntField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec + indentVec, nodeContentSize), targetTree.tree[nodeIdx].researchCost);
            EditorGUI.LabelField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec * 2, nodeLabelSize), "Invested");
            targetTree.tree[nodeIdx].researchInvested = EditorGUI.IntField(new Rect(targetTree.tree[nodeIdx].UIposition - scrollPosition + nextLineVec * 2 + indentVec, nodeContentSize), targetTree.tree[nodeIdx].researchInvested);
            EditorGUI.EndFoldoutHeaderGroup();

            //draw connections
            foreach(Tech req in targetTree.tree[nodeIdx].requirements)
            {
                int reqIdx = targetTree.FindTechIndex(req);
                if (reqIdx != -1)
                {
                    //draw connecting curvature
                    Handles.DrawBezier(targetTree.tree[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec,
                        targetTree.tree[reqIdx].UIposition - scrollPosition + incomingEdgeVec,
                        targetTree.tree[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec + Vector2.left * 100,
                        targetTree.tree[reqIdx].UIposition - scrollPosition + incomingEdgeVec + Vector2.right * 100,
                        Color.white,
                        null,
                        3f);

                    //draw arrow
                    Handles.DrawLine(targetTree.tree[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec, targetTree.tree[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec + upArrowVec);
                    Handles.DrawLine(targetTree.tree[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec, targetTree.tree[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec + downArrowVec);
                }
                else
                    Debug.Log("missing tech " + req.name);
            }

            //if cursor on node
            if(nodeRect.Contains(currentEvent.mousePosition))
            {
                if(UIEvent == EventType.MouseDown)
                {
                    //set active node
                    //left click
                    if (currentEvent.button == 0)
                    {
                        activeNode = targetTree.tree[nodeIdx];
                        mouseSelectionOffset = activeNode.UIposition - currentEvent.mousePosition;
                    }
                    else 
                    
                    //right click
                    if(currentEvent.button == 1)
                    {
                        selectedNode = targetTree.tree[nodeIdx];
                        Repaint();
                    }
                }

                else
                
                //create destroy connections
                if(UIEvent == EventType.MouseUp) // on mouse release
                {
                    //and selectnode is not empty
                    if(currentEvent.button == 1 && selectedNode != null && selectedNode != targetTree.tree[nodeIdx]) 
                    {
                        //remove connections
                        if (targetTree.tree[nodeIdx].requirements.Contains(selectedNode.tech))
                            targetTree.tree[nodeIdx].requirements.Remove(selectedNode.tech);
                        else if (selectedNode.requirements.Contains(targetTree.tree[nodeIdx].tech))
                            selectedNode.requirements.Remove(targetTree.tree[nodeIdx].tech);
                        else

                        //otherwise make connections
                        if (targetTree.IsConnectible(targetTree.tree.IndexOf(selectedNode), nodeIdx))
                        {
                            targetTree.tree[nodeIdx].requirements.Add(selectedNode.tech);

                            //make sure it didnt fuck up other connections
                            for (int k = 0; k < targetTree.tree.Count; k++)
                                targetTree.CorrectRequirementsCascades(k);
                        }
                    }
                }
            }
        }

        //scroll with middle mouse button
        if(currentEvent.button == 2)
        {
            //if button down
            if (currentEvent.type == EventType.MouseDown)
                //store coordinate
                scrollStartPos = (currentEvent.mousePosition + scrollPosition);
            //if held down
            else if (currentEvent.type == EventType.MouseDrag)
            {
                scrollPosition = -(currentEvent.mousePosition - scrollStartPos);// move the screen
                Repaint();//repaint gui
            }
        }

        //draw guiding connections on mouse hold pos
        //if right mouse hold and no selection
        if (selectedNode != null && currentEvent.button == 1)
        {
            //draw connection between selected node and mouse pos
            Handles.DrawBezier(currentEvent.mousePosition,
                selectedNode.UIposition - scrollPosition + incomingEdgeVec,
                currentEvent.mousePosition + Vector2.left * 100,
                selectedNode.UIposition - scrollPosition + incomingEdgeVec + Vector2.right * 100,
                Color.white,
                null,
                1.5f);
            Repaint();
        }

        //move nodes with left click
        //if you let go
        if(UIEvent == EventType.MouseUp)
        {
            //drop it
            activeNode = null;
        }
        else 

        if(UIEvent == EventType.MouseDrag) 
            if(activeNode != null)
                activeNode.UIposition = currentEvent.mousePosition + mouseSelectionOffset;

        //import new tech
        if (currentEvent.type == EventType.DragUpdated)
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        else if(currentEvent.type == EventType.DragPerform)
        {
            for(int i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                if (DragAndDrop.objectReferences[i] is Tech)
                    targetTree.AddNode(DragAndDrop.objectReferences[i] as Tech, currentEvent.mousePosition + scrollPosition);
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
