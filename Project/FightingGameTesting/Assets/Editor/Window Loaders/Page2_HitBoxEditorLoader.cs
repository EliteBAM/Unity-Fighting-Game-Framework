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

    Texture2D darkLineStyleTexture;
    GUIStyle darkLineStyle;

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

        // Create and apply the texture and style for the dark line.
        darkLineStyleTexture = new Texture2D(1, 1);
        darkLineStyleTexture.SetPixel(0, 0, new Color(0.15f, 0.15f, 0.15f, 1));  // You can adjust this color.
        darkLineStyleTexture.Apply();

        darkLineStyle = new GUIStyle();
        darkLineStyle.normal.background = darkLineStyleTexture;
        darkLineStyle.margin = new RectOffset(10, 10, 0, 0);  // Horizontal space from the edges.

        hitBoxItemStyle = new GUIStyle();
        hitBoxItemStyle.normal.background = hitBoxItemStyleTexture;
        hitBoxItemStyle.padding = new RectOffset(10, 10, 10, 10);  // adjust these values for desired padding
        hitBoxItemStyle.border = new RectOffset(13, 13, 13, 13); // Adjust as needed for the specific texture.

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
        //EditorGUILayout.BeginHorizontal();

        //top border
        GUILayout.BeginVertical(darkLineStyle);
        GUILayout.Space(2);

        //left border
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(2);

        // Start a vertical group with the border style
        EditorGUILayout.BeginVertical(hitBoxItemStyle);

        // Row 1
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name: ", GUILayout.Width(40)); // Adjust the width as necessary
        HitBoxEditorManager.instance.hitBoxNames[index] = EditorGUILayout.TextField(name, GUILayout.Width(50)); // Adjust the width as necessary
        EditorGUILayout.Space(3);

        if(HitBoxEditorManager.instance.hitBoxData[index].boxType == HitBoxType.Hit)
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
        GUILayout.EndHorizontal();
        //row 1

        EditorGUILayout.Space(3);

        // Row 2
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Start frame: ", GUILayout.Width(80)); // Adjust the width as necessary
        item = HitBoxEditorManager.instance.ChangeStartFrame(item, EditorGUILayout.IntField(item.startFrame, GUILayout.Width(50))); // Adjust the width as necessary
        EditorGUILayout.LabelField("End frame: ", GUILayout.Width(80)); // Adjust the width as necessary
        item = HitBoxEditorManager.instance.ChangeEndFrame(item, EditorGUILayout.IntField(item.endFrame, GUILayout.Width(50))); // Adjust the width as necessary
        GUILayout.EndHorizontal();
        //row 2

        HitBoxEditorManager.instance.hitBoxData[index] = item;

        EditorGUILayout.EndVertical(); //end of inner box

        //right border
        EditorGUILayout.Space(2);
        EditorGUILayout.EndHorizontal();

        //bottom border
        GUILayout.Space(2);
        GUILayout.EndVertical();

        //EditorGUILayout.Space(12);
        //EditorGUILayout.EndHorizontal();


        //space between scroll menu items
        GUILayout.Space(2);
    }
}
