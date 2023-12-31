using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using System;

[ExecuteInEditMode]
public class LoadEditorLayout : MonoBehaviour
{
    public string layoutPath;
    // Start is called before the first frame update
    void Awake()
    {
        LayoutUtility.LoadLayout(layoutPath);
        DestroyImmediate(gameObject);
    }

}

public static class LayoutUtility
{

    private enum MethodType { Save, Load };

    static MethodInfo GetMethod(MethodType method_type)
    {

        Type layout = Type.GetType("UnityEditor.WindowLayout,UnityEditor");

        MethodInfo save = null;
        MethodInfo load = null;

        if (layout != null)
        {
            load = layout.GetMethod("LoadWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(bool) }, null);
            save = layout.GetMethod("SaveWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
        }

        if (method_type == MethodType.Save)
        {
            return save;
        }
        else
        {
            return load;
        }

    }

    public static void SaveLayout(string path)
    {
        path = Path.Combine(Directory.GetCurrentDirectory(), path);
        GetMethod(MethodType.Save).Invoke(null, new object[] { path });
    }

    public static void LoadLayout(string path)
    {
        path = Path.Combine(Directory.GetCurrentDirectory(), path);
        GetMethod(MethodType.Load).Invoke(null, new object[] { path, false });
    }

}
