using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Page2_HitBoxEditorLoader : IHitBoxEditorWindowLoader
{
    public HitBoxEditorWindow Window { get; set; }

    //GUI styles and custom data
    Texture2D hitBoxItemStyleTexture;
    GUIStyle hitBoxItemStyle;

    Texture2D keyFramStyleTexture;
    GUIStyle keyFrameStyle;

    Texture2D darkLineStyleTexture;
    GUIStyle darkLineStyle;

    Texture2D lightLineStyleTexture;
    GUIStyle lightLineStyle;

    Texture2D selectedLineTexture;
    GUIStyle selectedLineStyle;

    GUIStyle scrollViewStyle;
    GUIStyle horizontalScrollbarStyle;
    private Vector2 scrollPosition;

    public void Initialize(HitBoxEditorWindow window)
    {
        Window = window;

        // Create and apply the texture and style for the hitbox item.
        hitBoxItemStyleTexture = new Texture2D(1, 1);
        hitBoxItemStyleTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1));  // You can adjust this color.
        hitBoxItemStyleTexture.Apply();

        hitBoxItemStyle = new GUIStyle();
        hitBoxItemStyle.normal.background = hitBoxItemStyleTexture;
        hitBoxItemStyle.padding = new RectOffset(10, 10, 10, 10);  // adjust these values for desired padding
        hitBoxItemStyle.border = new RectOffset(13, 13, 13, 13); // Adjust as needed for the specific texture.

        // Create and apply the texture and style for the hitbox item.
        keyFramStyleTexture = new Texture2D(1, 1);
        keyFramStyleTexture.SetPixel(0, 0, new Color(1f, 0.55f, 0f, 1));  // You can adjust this color.
        keyFramStyleTexture.Apply();

        keyFrameStyle = new GUIStyle();
        keyFrameStyle.normal.background = keyFramStyleTexture;
        keyFrameStyle.margin = new RectOffset(0, 0, 0, 0);  // Horizontal space from the edges.

        // Create and apply the texture and style for the dark line.
        darkLineStyleTexture = new Texture2D(1, 1);
        darkLineStyleTexture.SetPixel(0, 0, new Color(0.15f, 0.15f, 0.15f, 1));  // You can adjust this color.
        darkLineStyleTexture.Apply();

        darkLineStyle = new GUIStyle();
        darkLineStyle.normal.background = darkLineStyleTexture;
        darkLineStyle.margin = new RectOffset(0, 0, 0, 0);  // Horizontal space from the edges.

        // Create and apply the texture and style for the dark line.
        lightLineStyleTexture = new Texture2D(1, 1);
        lightLineStyleTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1));  // You can adjust this color.
        lightLineStyleTexture.Apply();

        lightLineStyle = new GUIStyle();
        lightLineStyle.normal.background = lightLineStyleTexture;
        lightLineStyle.margin = new RectOffset(0, 0, 0, 0);  // Horizontal space from the edges.

        // Create and apply the texture and style for the dark line.
        selectedLineTexture = new Texture2D(1, 1);
        selectedLineTexture.SetPixel(0, 0, new Color(0f, 1f, 0f, 1));  // You can adjust this color.
        selectedLineTexture.Apply();

        selectedLineStyle = new GUIStyle();
        selectedLineStyle.normal.background = selectedLineTexture;
        selectedLineStyle.margin = new RectOffset(0, 0, 0, 0);  // Horizontal space from the edges.


        //scroll style
        scrollViewStyle = new GUIStyle(GUI.skin.scrollView);
        scrollViewStyle.padding.right += 15; // Adjust the value 20 based on the scrollbar width.
        horizontalScrollbarStyle = new GUIStyle(GUI.skin.horizontalScrollbar);
        horizontalScrollbarStyle.fixedHeight = 0;
    }

    public void SetUpWindow()
    {
        Window.minSize = new Vector2(350, 400);
        Window.maxSize = new Vector2(350, 400);
        Window.Show();
    }

    public void UpdateGUI()
    {

        DisplayTopAnchoredArea();

        DisplayDivider(5, 15);


        LoadScrollArea();


        DisplayDivider(15, 5);

        DisplayBottomAnchoredArea();
    }

    public void LoadScrollArea()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, scrollViewStyle,
            GUILayout.Width(Window.minSize.x * 0.95f));

        // All content here will be part of the scrollable area.
        for (int i = 0; i < HitBoxEditorManager.instance.hitBoxData.Count; i++)
        {
            DisplayHitboxMenuItem(HitBoxEditorManager.instance.hitBoxNames[i], HitBoxEditorManager.instance.hitBoxData[i], i);
        }
        Window.Repaint();

        EditorGUILayout.EndScrollView();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void DisplayTopAnchoredArea()
    {
        GUILayout.BeginVertical("box"); // "box" style to give it a distinct look
        GUILayout.Label("HitBox Tools:");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally

        GUILayout.BeginVertical();


        if (GUILayout.Button("Add HitBox", GUILayout.Width(Window.minSize.x * 0.9f), GUILayout.Height(Window.minSize.y / 12)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
            HitBoxEditorManager.instance.AddHitBox();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Remove HitBox", GUILayout.Width(Window.minSize.x * 0.9f), GUILayout.Height(Window.minSize.y / 12)))
        {
            HitBoxEditorManager.instance.RemoveHitBox();
        }


        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.EndVertical();
    }

    void DisplayDivider(int topSpace, int botSpace)
    {
        GUILayout.Space(topSpace);  // Add a bit of space before and after the separator
        GUILayout.Box("", darkLineStyle, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(2) });
        GUILayout.Space(botSpace);  // Add a bit of space after the separator
    }

    void DisplayBottomAnchoredArea()
    {
        // This is an example. You can add your desired GUI elements here.
        GUILayout.BeginVertical("box"); // "box" style to give it a distinct look
        GUILayout.Label("Save Options:");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally

        if (GUILayout.Button("Save Changes", GUILayout.Width(Window.minSize.x * 0.9f / 3), GUILayout.Height(Window.minSize.y / 12f)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
            HitBoxEditorManager.instance.hitBoxData.Add(new HitBoxData());
        }

        if (GUILayout.Button("Save and Exit", GUILayout.Width(Window.minSize.x * 0.9f / 3), GUILayout.Height(Window.minSize.y / 12f)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
            HitBoxEditorManager.instance.hitBoxData.Add(new HitBoxData());
        }

        if (GUILayout.Button("Exit", GUILayout.Width(Window.minSize.x * 0.9f / 3), GUILayout.Height(Window.minSize.y / 12f)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
            HitBoxEditorManager.instance.hitBoxData.Add(new HitBoxData());
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }


    void DisplayHitboxMenuItem(string name, HitBoxData item, int index)
    {
        int frameIndex = Array.IndexOf(item.sizeChangeFrames, HitBoxEditorManager.instance.currentFrame);

        // Start of outer vertical for top border
        if (HitBoxEditorManager.instance.hitBoxes[index].isSelected)
            EditorGUILayout.BeginVertical(selectedLineStyle);
        else if (!HitBoxEditorManager.instance.hitBoxes[index].isSelected && frameIndex != -1)
            EditorGUILayout.BeginVertical(keyFrameStyle);
        else
            EditorGUILayout.BeginVertical(darkLineStyle);

        // Top border
        EditorGUILayout.Space(2);

        // Start of outer horizontal for left border
        EditorGUILayout.BeginHorizontal();

        // Left border
        EditorGUILayout.Space(2);

        // INNER CONTENT AND BORDERS 

        // Top border 2
        if (HitBoxEditorManager.instance.hitBoxes[index].isSelected && frameIndex != -1)
            EditorGUILayout.BeginVertical(keyFrameStyle);
        else
            EditorGUILayout.BeginVertical(lightLineStyle);
        EditorGUILayout.Space(2);

        // Left Border 2
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(2);

        EditorGUILayout.BeginVertical(hitBoxItemStyle);

        // Row 1
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name: ", GUILayout.Width(40)); // Adjust the width as necessary
        HitBoxEditorManager.instance.hitBoxNames[index] = EditorGUILayout.TextField(name, GUILayout.Width(50)); // Adjust the width as necessary
        EditorGUILayout.Space(3);

        if (HitBoxEditorManager.instance.hitBoxData[index].boxType == HitBoxType.Hit)
        {
            if (GUILayout.Button("Hit"))
            {
                Debug.Log("hit button pressed");
                Debug.Log("Switching to hurtbox");
                item = HitBoxEditorManager.instance.ChangeHitBoxTypeData(index, HitBoxType.Hurt);
            }
        }
        else
        {
            if (GUILayout.Button("Hurt"))
            {
                Debug.Log("hurt button pressed");
                Debug.Log("Switching to hitbox");
                item = HitBoxEditorManager.instance.ChangeHitBoxTypeData(index, HitBoxType.Hit);
            }
        }

        if (GUILayout.Button("Set Start"))
        {
            item = HitBoxEditorManager.instance.ChangeStartFrame(item, HitBoxEditorManager.instance.currentFrame);
        }
        if (GUILayout.Button("Set End"))
        {
            item = HitBoxEditorManager.instance.ChangeEndFrame(item, HitBoxEditorManager.instance.currentFrame);
        }
        EditorGUILayout.EndHorizontal();
        //row 1

        EditorGUILayout.Space(3);

        // Row 2
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Start frame: ", GUILayout.Width(80)); // Adjust the width as necessary
        item = HitBoxEditorManager.instance.ChangeStartFrame(item, EditorGUILayout.IntField(item.startFrame, GUILayout.Width(50))); // Adjust the width as necessary
        EditorGUILayout.LabelField("End frame: ", GUILayout.Width(80)); // Adjust the width as necessary
        item = HitBoxEditorManager.instance.ChangeEndFrame(item, EditorGUILayout.IntField(item.endFrame, GUILayout.Width(50))); // Adjust the width as necessary
        GUILayout.EndHorizontal();
        //row 2

        // Row 3
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove Current Resize Frame"))
        {
            item = HitBoxEditorManager.instance.RemoveSizeChangeFrame(item);
        }
        GUILayout.EndHorizontal();
        //row 3

        HitBoxEditorManager.instance.hitBoxData[index] = item;

        EditorGUILayout.EndVertical(); //end of inner box   

        // Right Border 2
        EditorGUILayout.Space(2);
        EditorGUILayout.EndHorizontal(); // Close Left Border 2 horizontal

        // Bottom Border 2
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical(); // Close Top Border 2 vertical

        // END OF INNER CONTENT AND BORDERS

        // Right border of outer border
        EditorGUILayout.Space(2);

        // End outer horizontal for right border
        EditorGUILayout.EndHorizontal();

        // Bottom border of outer border
        EditorGUILayout.Space(2);

        // End outer vertical for bottom border
        EditorGUILayout.EndVertical();

        // Space between scroll menu items
        GUILayout.Space(2);
    }
}
