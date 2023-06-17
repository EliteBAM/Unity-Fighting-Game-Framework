using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//[ExecuteInEditMode]
public class HitBoxEditor : MonoBehaviour
{

    [Header("Edit Fight Animation HitBoxes:")]
    public FightAnimationPlayableAsset fightAnimation;

    [Header("Create New Fight Animation From Clip:")]
    public AnimationClip clip;

    [Header("File Settings:")]
    public string fileName;


    //Timeline
    private PlayableDirector director;
    private TimelineAsset timeline;
    private AnimationTrack track;
    private bool clipAdded = false;

    //Scene objects
    private GameObject rig;
    private GameObject cam;

    //Camera Variables
    Vector3 defaultCameraPosition;


    void Awake()
    {

        //init scene objects
        rig = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        defaultCameraPosition = new Vector3(0, 13, 76);

        //init timeline objects
        director = rig.GetComponent<PlayableDirector>();
        timeline = (TimelineAsset)director.playableAsset;
        track = (AnimationTrack)timeline.GetOutputTrack(0);

        ResetCamera();

    }

    private void Update()
    {
        if(fightAnimation != null)
        {
            clip = fightAnimation.clip;
            if(!clipAdded)
            {
                track.CreateClip(clip);
                clipAdded = true;
            }
        }
    }

    public void ResetCamera()
    {
        cam.transform.position = defaultCameraPosition;
        cam.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void PlayPlayableDirector()
    {
        director.Play();
    }

    public void GeneratePlayableAssetFile()
    {
        PlayableAsset newAsset = new FightAnimationPlayableAsset(clip);
        AssetDatabase.CreateAsset(newAsset, "Assets/Animations/Test Moveset/" + fileName + ".playable");
    }
}
