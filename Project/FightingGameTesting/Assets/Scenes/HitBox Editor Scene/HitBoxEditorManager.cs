using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;


public class HitBoxEditorManager : MonoBehaviour
{

    public static HitBoxEditorManager instance;

    private FightAnimationPlayableAsset _fightAnimationAsset;
    public FightAnimationPlayableAsset FightAnimationAsset {
        get => _fightAnimationAsset;
        set
        {
            _fightAnimationAsset = value;

            Debug.Log("HitBox Editor Service: FightAnimation File Recieved!");
        }
    }

    private bool timelineInitialized = false;
    private bool editorDataInitialized = false;

    [Header("Fight Animation Data")]
    public AnimationClip clip;
    public List<HitBoxData> hitBoxData;
    public List<string> hitBoxNames;

    [Header("HitBox Editor Info")]
    public int currentFrame;
    public int frameRate;

    //hitbox management
    private EditorHitBox previousSelection;

    //Timeline
    public PlayableDirector director;
    public TimelineAsset timeline;
    public AnimationTrack track;
    TimelineClip timelineClip;

    //Scene objects
    private GameObject rig;
    private GameObject cam;
    private Animator animator;
    public List<EditorHitBox> hitBoxes;

    //Camera Variables
    Vector3 defaultCameraPosition;


    void Awake()
    {
        InitializeSingleton();
    }

    void Start()
    {
        hitBoxData = new List<HitBoxData>();
        hitBoxNames = new List<string>();
        hitBoxes = new List<EditorHitBox>();

        //init scene objects
        Debug.Log("HitBox Editor Started");
        rig = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = rig.GetComponent<Animator>();
        defaultCameraPosition = new Vector3(0, 13, 76);

        ResetCamera();

    }
    private void Update()
    {

        InitializeEditorData();

        InitializeTimeline();

        if(timelineInitialized && editorDataInitialized)
        {

            currentFrame = (int)(director.time * 60);

        }

        SelectHitboxes();

    }
    void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("runtime singleton set");
        }
    }

    void InitializeEditorData()
    {
        if (FightAnimationAsset != null && !editorDataInitialized)
        {
            clip = FightAnimationAsset.clip;
            frameRate = (int)clip.frameRate;

            if (FightAnimationAsset.hitBoxData != null && FightAnimationAsset.hitBoxNames != null)
            {
                for (int i = 0; i < FightAnimationAsset.hitBoxData.Length; i++)
                {
                    hitBoxData.Add(FightAnimationAsset.hitBoxData[i]);
                    hitBoxNames.Add(FightAnimationAsset.hitBoxNames[i]);
                    GameObject newHitBoxObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newHitBoxObject.transform.localScale = Vector3.one * 5;
                    newHitBoxObject.transform.position = rig.transform.position + (Vector3.up * 10);
                    EditorHitBox script = newHitBoxObject.AddComponent<EditorHitBox>();
                    script.index = i;

                    hitBoxes.Add(script);
                }
            }

            editorDataInitialized = true;
        }
    }

    void InitializeTimeline()
    {
        if (!timelineInitialized && _fightAnimationAsset != null)
        {
            //init timeline objects
            director = rig.GetComponent<PlayableDirector>();
            if (director != null)
            {
                director.playableAsset = new TimelineAsset();
                timeline = (TimelineAsset)director.playableAsset;
                
                Debug.Log("timeline asset created");

                if (timeline != null)
                {
                    // Create an AnimationTrack in the timeline
                    track = timeline.CreateTrack<AnimationTrack>(null, "AnimationTrackName");

                    if (track != null)
                    {
                        Debug.Log("track created");

                        // Assuming 'clip' is your AnimationClip object you want to add
                        if (clip != null)
                        {
                            timelineClip = track.CreateClip(clip);
                            timelineClip.start = 0;
                            timelineClip.duration = clip.length;
                            Debug.Log("clip added to track");
                        }
                        else
                        {
                            Debug.Log("No animation clip available");
                        }

                        // Assign the target Animator to the AnimationTrack
                        Animator targetAnimator = animator;  // Assuming 'rig' has the Animator component you want to target
                        if (targetAnimator != null)
                        {
                            director.SetGenericBinding(track, targetAnimator);
                            targetAnimator.applyRootMotion = false;
                            Debug.Log("Animator assigned to track");
                        }
                        else
                        {
                            Debug.Log("No animator found on the GameObject");
                        }

                    }
                }
            }

            Selection.activeObject = rig;

            timelineInitialized = true;
        }
    }

    void SelectHitboxes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Shoot the ray and check if it hits any collider
            if (Physics.Raycast(ray, out hit))
            {
                // Output the name of the object clicked on
                Debug.Log("You clicked on: " + hit.transform.name);

                EditorHitBox hitBox = hit.transform.GetComponent<EditorHitBox>();
                if (hitBox != null)
                {
                    Debug.Log("its a hitbox");
                    if(previousSelection != null && previousSelection != hitBox)
                        previousSelection.isSelected = false;
                    hitBox.isSelected = !hitBox.isSelected;
                }
                if (!hit.transform.tag.Equals("handle"))
                    previousSelection = hitBox;

            }
            else
            {                
                if (previousSelection != null)
                    previousSelection.isSelected = false;
                previousSelection = null;
            }
        }
    }

    public void AddHitBox()
    {
        HitBoxData newHitBoxData = new HitBoxData();
        newHitBoxData.startFrame = 0;
        newHitBoxData.endFrame = 1;
        newHitBoxData.sizeChangeFrames = new int[0];
        newHitBoxData.center = new Vector2[0];
        newHitBoxData.size = new Vector2[0];

        hitBoxData.Add(newHitBoxData);
        hitBoxNames.Add("new Hitbox");

        GameObject newHitBoxObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newHitBoxObject.transform.localScale = Vector3.one * 5;
        newHitBoxObject.transform.position = rig.transform.position + (Vector3.up * 10);
        EditorHitBox script = newHitBoxObject.AddComponent<EditorHitBox>();
        script.index = hitBoxData.Count - 1;

        hitBoxes.Add(script);
    }

    public void RemoveHitBox()
    {
        if(previousSelection != null)
        {
            hitBoxData.Remove(hitBoxData[previousSelection.index]);
            hitBoxNames.Remove(hitBoxNames[previousSelection.index]);
            hitBoxes.Remove(previousSelection);

            Destroy(previousSelection.handle1);
            Destroy(previousSelection.handle2);
            Destroy(previousSelection.gameObject);

            for(int i = previousSelection.index; i < hitBoxes.Count; i++) //update index of succeeding hitboxes to account for removal of a previous index
                hitBoxes[i].index = i;

            previousSelection = null;
        }
        else
            Debug.Log("HitBox Not Removed. No HitBox Was Selected.");
    }

    public HitBoxData AppendSizeData(int hitBoxIndex, Vector2 center, Vector2 size)
    {
        HitBoxData hitBoxData = this.hitBoxData[hitBoxIndex];

        int frameIndex = Array.IndexOf(hitBoxData.sizeChangeFrames, currentFrame);
        Debug.Log("frame index of current frame: " + frameIndex);
        if (frameIndex != -1)
        {
            hitBoxData.center[frameIndex] = center;
            hitBoxData.size[frameIndex] = size;
        }
        else
        {
            //append frame list
            int[] newTimeChangeFrames = new int[hitBoxData.sizeChangeFrames.Length + 1];
            for (int i = 0; i < hitBoxData.sizeChangeFrames.Length; i++)
                newTimeChangeFrames[i] = hitBoxData.sizeChangeFrames[i];

            newTimeChangeFrames[newTimeChangeFrames.Length - 1] = (int)currentFrame;
            hitBoxData.sizeChangeFrames = newTimeChangeFrames;

            //append center data list
            Vector2[] newCenterData = new Vector2[hitBoxData.center.Length + 1];
            for (int i = 0; i < hitBoxData.center.Length; i++)
                newCenterData[i] = hitBoxData.center[i];

            newCenterData[newCenterData.Length - 1] = center;
            hitBoxData.center = newCenterData;

            //append size data list
            Vector2[] newSizeData = new Vector2[hitBoxData.size.Length + 1];
            for (int i = 0; i < hitBoxData.size.Length; i++)
                newSizeData[i] = hitBoxData.size[i];

            newSizeData[newSizeData.Length - 1] = size;
            hitBoxData.size = newSizeData;
        }

        return hitBoxData;
    }

    public HitBoxData ChangeHitBoxTypeData(int hitBoxIndex, HitBoxType type)
    {
        HitBoxData hitBoxData = this.hitBoxData[hitBoxIndex];

        hitBoxData.boxType = type;

        Debug.Log("The hitbox type has been changed to: " + hitBoxData.boxType);

        return hitBoxData;
    }

    public HitBoxData ChangeStartFrame(HitBoxData hitBoxData, int startFrame)
    {
        if(startFrame < hitBoxData.endFrame)
            hitBoxData.startFrame = startFrame;

        Debug.Log("The hitbox startFrane has been changed to: " + hitBoxData.startFrame);

        return hitBoxData;
    }

    public HitBoxData ChangeEndFrame(HitBoxData hitBoxData, int endFrame)
    {
        if (endFrame > hitBoxData.startFrame)
            hitBoxData.endFrame = endFrame;

        Debug.Log("The hitbox endFrame has been changed to: " + hitBoxData.endFrame);

        return hitBoxData;
    }

    public HitBoxData RemoveSizeChangeFrame(HitBoxData hitBoxData)
    {
        int frame = currentFrame + 1;

        int frameIndex;

        do
        {
            frameIndex = Array.IndexOf(hitBoxData.sizeChangeFrames, --frame);
        } while (frameIndex == -1 && frame > 0);

        Debug.Log("frame index of current frame: " + frameIndex);
        if (frameIndex != -1)
        {
            //append frame list
            int[] newSizeChangeFrames = new int[hitBoxData.sizeChangeFrames.Length - 1];
            for (int i = 0; i < hitBoxData.sizeChangeFrames.Length; i++)
            {
                if (i < frameIndex)
                    newSizeChangeFrames[i] = hitBoxData.sizeChangeFrames[i];
                else if(i > frameIndex)
                    newSizeChangeFrames[i - 1] = hitBoxData.sizeChangeFrames[i];
            }

            hitBoxData.sizeChangeFrames = newSizeChangeFrames;

            //append center data list
            Vector2[] newCenterData = new Vector2[hitBoxData.center.Length - 1];
            for (int i = 0; i < hitBoxData.center.Length; i++)
            {
                if (i < frameIndex)
                    newCenterData[i] = hitBoxData.center[i];
                else if (i > frameIndex)
                    newCenterData[i - 1] = hitBoxData.center[i];
            }

            hitBoxData.center = newCenterData;

            //append size data list
            Vector2[] newSizeData = new Vector2[hitBoxData.size.Length - 1];
            for (int i = 0; i < hitBoxData.size.Length; i++)
            {
                if (i < frameIndex)
                    newSizeData[i] = hitBoxData.size[i];
                else if (i > frameIndex)
                    newSizeData[i - 1] = hitBoxData.size[i];
            }

            hitBoxData.size = newSizeData;
        }
        else
        {
            Debug.Log("No Key Frame Exists On This Frame");
        }

        return hitBoxData;
    }

    public void SaveData()
    {
        _fightAnimationAsset.hitBoxData = hitBoxData.ToArray();
        _fightAnimationAsset.hitBoxNames = hitBoxNames.ToArray();

        EditorUtility.SetDirty(_fightAnimationAsset);
        AssetDatabase.SaveAssets();
    }

    public void ResetCamera()
    {
        cam.transform.position = defaultCameraPosition;
        cam.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

}
