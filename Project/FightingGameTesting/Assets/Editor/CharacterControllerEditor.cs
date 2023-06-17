using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterController))]
public class CharacterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        CharacterController targetScript = (CharacterController)target;

        GUILayout.Space(15);

        GUILayout.Label("Playable Graph Visualizer:", EditorStyles.boldLabel);

        GUILayout.Space(5);

        if (GUILayout.Button("Show Animation Graph"))
        {
            targetScript.DisplayAnimationGraph();
        }
    }
}