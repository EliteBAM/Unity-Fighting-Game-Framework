using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Page1_HitBoxEditorLoader : IHitBoxEditorWindowLoader
{
    public HitBoxEditorWindow Window { get; set; }


    public FightAnimationPlayableAsset fightAnimation;

    public AnimationClip clip;


    public void Initialize(HitBoxEditorWindow window)
    {
        Window = window;
    }

    public void SetUpWindow()
    {
        Window.minSize = new Vector2(350, 400);
        Window.maxSize = new Vector2(350, 400);
        Window.Show();
    }

    public void UpdateGUI()
    {
        Window.maxSize = new Vector2(1000, 1000);

        // Store the old label width
        float oldLabelWidth = EditorGUIUtility.labelWidth;

        // Set the new label width
        EditorGUIUtility.labelWidth = 100;

        // --- Section 1: Exposed Property with Open Animation Button ---
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();  // Center Vertically

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally
        GUILayout.Label("Edit Existing FightAnimation File");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally
        fightAnimation = (FightAnimationPlayableAsset)EditorGUILayout.ObjectField("Fight Animation:", fightAnimation, typeof(FightAnimationPlayableAsset), false, GUILayout.Width(325));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10); // Small space between the ObjectField and the button

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally

        if (GUILayout.Button("Open FightAnimation", GUILayout.Width(Window.minSize.x * 0.9f), GUILayout.Height(Window.minSize.y / 8f)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
        
            if(fightAnimation != null)
            {
                HitBoxEditorManager.instance.FightAnimationAsset = fightAnimation;

                HitBoxEditorWindow.loader = new Page2_HitBoxEditorLoader();
                HitBoxEditorWindow.loader.Initialize(Window);
            }
            else
                Debug.LogWarning("No File Selected");

        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // --- Section 2: Visual Horizontal Line Break ---

        // Create and apply the texture and style for the dark line.
        Texture2D darkLineTexture = new Texture2D(1, 1);
        darkLineTexture.SetPixel(0, 0, new Color(0.15f, 0.15f, 0.15f, 1));  // You can adjust this color.
        darkLineTexture.Apply();

        GUIStyle darkLineStyle = new GUIStyle();
        darkLineStyle.normal.background = darkLineTexture;
        darkLineStyle.normal.background = darkLineTexture;
        darkLineStyle.margin = new RectOffset(10, 10, 0, 0);  // Horizontal space from the edges.


        GUILayout.Space(15);  // Add a bit of space before and after the separator
        GUILayout.Box("", darkLineStyle, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(2) });
        GUILayout.Space(15);  // Add a bit of space after the separator

        // --- Section 3: Create Animation Button ---
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally
        GUILayout.Label("Create New File from Animation Clip");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally
        clip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip:", clip, typeof(AnimationClip), false, GUILayout.Width(325));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10); // Small space between the ObjectField and the button

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();  // Center Horizontally

        if (GUILayout.Button("Create New FightAnimation", GUILayout.Width(Window.minSize.x * 0.9f), GUILayout.Height(Window.minSize.y / 8f)))  // Define the button width or remove GUILayout.Width if you want it to auto-size
        {
            if(clip != null)
            {
                //Create new fight animation, stored in HBE runtime service
                HitBoxEditorManager.instance.FightAnimationAsset = new FightAnimationPlayableAsset(clip);
                //save serialized asset of this instance in target directory
                AssetDatabase.CreateAsset(HitBoxEditorManager.instance.FightAnimationAsset, "Assets/Animations/Test Moveset/" + clip.name + ".playable");

                HitBoxEditorWindow.loader = new Page2_HitBoxEditorLoader();
                HitBoxEditorWindow.loader.Initialize(Window);
            }
            else
                Debug.LogWarning("No File Selected");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        // Restore the old label width
        EditorGUIUtility.labelWidth = oldLabelWidth;
    }

}
