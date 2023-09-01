using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EnterPlayMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("enter play mode");
        EditorApplication.EnterPlaymode();
        DestroyImmediate(this);
#endif
    }

}
