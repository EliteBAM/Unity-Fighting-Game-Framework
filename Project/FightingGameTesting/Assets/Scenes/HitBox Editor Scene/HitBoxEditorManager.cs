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
    public GameObject rig;
    private GameObject cam;
    private Animator animator;
    private GameObject[] hitBoxes;

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
                    newHitBoxObject.AddComponent<EditorHitBox>();
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
        newHitBoxData.sizeChangeFrames = new int[0];
        newHitBoxData.sizeChangeFrameData = new (Vector2, Vector2)[0];

        hitBoxData.Add(newHitBoxData);
        hitBoxNames.Add("new Hitbox");

        GameObject newHitBoxObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newHitBoxObject.transform.localScale = Vector3.one * 5;
        newHitBoxObject.transform.position = rig.transform.position + (Vector3.up * 10);
        EditorHitBox script = newHitBoxObject.AddComponent<EditorHitBox>();
        script.index = hitBoxData.Count - 1;
    }

    public HitBoxData AppendSizeData(int hitBoxIndex, Vector2 center, Vector2 size)
    {
        HitBoxData hitBoxData = this.hitBoxData[hitBoxIndex];

        int frameIndex = Array.IndexOf(hitBoxData.sizeChangeFrames, currentFrame);
        Debug.Log("frame index of current frame: " + frameIndex);
        if (frameIndex != -1)
        {
            hitBoxData.sizeChangeFrameData[frameIndex] = (center, size);
        }
        else
        {
            //append frame list
            int[] newTimeChangeFrames = new int[hitBoxData.sizeChangeFrames.Length + 1];
            for (int i = 0; i < hitBoxData.sizeChangeFrames.Length; i++)
                newTimeChangeFrames[i] = hitBoxData.sizeChangeFrames[i];

            newTimeChangeFrames[newTimeChangeFrames.Length - 1] = (int)currentFrame;
            hitBoxData.sizeChangeFrames = newTimeChangeFrames;

            //append data list
            (Vector2 Center, Vector2 Size)[] newSizeChangeFrameData = new (Vector2 Center, Vector2 Size)[hitBoxData.sizeChangeFrameData.Length + 1];
            for (int i = 0; i < hitBoxData.sizeChangeFrameData.Length; i++)
                newSizeChangeFrameData[i] = hitBoxData.sizeChangeFrameData[i];

            newSizeChangeFrameData[newSizeChangeFrameData.Length - 1] = (center, size);
            hitBoxData.sizeChangeFrameData = newSizeChangeFrameData;
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

    public void ResetCamera()
    {
        cam.transform.position = defaultCameraPosition;
        cam.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

}
