using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GoToScene_HitBoxEditorLoader : IHitBoxEditorWindowLoader
{
    public HitBoxEditorWindow Window { get; set; }

    public void Initialize(HitBoxEditorWindow window)
    {
        Window = window;
    }

    public void SetUpWindow()
    {
        Window.minSize = new Vector2(400, 400);
        Window.maxSize = new Vector2(400, 400);
        Window.Show();
    }

    public void UpdateGUI()
    {

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();  // Center Vertically

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally

        if (GUILayout.Button("Open HitBox Editor", GUILayout.Width(Window.minSize.x * 0.9f), GUILayout.Height(Window.minSize.y / 5)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
            EditorSceneManager.OpenScene("Assets/Scenes/HitBox Editor Scene/HitBox Editor.unity");
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();


    }
}
