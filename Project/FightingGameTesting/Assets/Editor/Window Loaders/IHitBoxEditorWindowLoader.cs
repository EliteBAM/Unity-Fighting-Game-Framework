using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public interface IHitBoxEditorWindowLoader
{ 
    HitBoxEditorWindow Window { get; set; }

    public void Initialize(HitBoxEditorWindow window);

    public void SetUpWindow();

    public void UpdateGUI();
}
