using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitBoxEditor))]
public class HitBoxEditorEditorExtension : Editor
{

    private void OnSceneGUI()
    {
        Debug.Log("OnSceneGUI called");
        Event e = Event.current;
        if (e.isKey)
        {
            //debug:
            Debug.Log("key read: " + e.keyCode);

            if (e.keyCode == KeyCode.Space)
            {
                Debug.Log("Space! Pressed");
            }
        }
    }

    public override void OnInspectorGUI()
    {
        HitBoxEditor targetScript = (HitBoxEditor)target;

        GUILayout.Space(20);

        if (GUILayout.Button("Play Animation"))
        {
            targetScript.PlayPlayableDirector();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Reset Camera"))
        {
            targetScript.ResetCamera();
        }
        GUIStyle bigBold = new GUIStyle() { 
            
            fontSize = 15,
                                            };

        GUILayout.Label("HITBOX EDITOR TOOLS", bigBold);

        DrawDefaultInspector();


        GUILayout.Space(20);

        if (GUILayout.Button("Save HitBox Playable Asset"))
        {
            targetScript.GeneratePlayableAssetFile();
        }



        
    }
}