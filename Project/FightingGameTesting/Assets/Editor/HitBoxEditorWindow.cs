using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public class HitBoxEditorWindow : EditorWindow
{
    public static IHitBoxEditorWindowLoader loader;

    [MenuItem("Window/Hit Box Editor")]
    static void InitializeWindow()
    {

        GetWindow(typeof(HitBoxEditorWindow)).name = "HitBox Editor";

    }

    private void OnEnable()
    {
        title = "HitBox Editor";

        SelectLoader();

        loader.SetUpWindow();
    }

    private void OnGUI()
    {
        if (loader != null)
            loader.UpdateGUI();
    }


    private void SelectLoader()
    {
        if (SceneManager.GetActiveScene().name == "HitBox Editor")
        {
            loader = new Page1_HitBoxEditorLoader();
            loader.Initialize(this);
        }
        else
        {
            loader = new GoToScene_HitBoxEditorLoader();
            loader.Initialize(this);
        }
    }
}
