using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class FightAnimationPlayableAsset : PlayableAsset
{

    public AnimationClip clip;

    public HitBoxData[] hitBoxData;
    
    //for use in hitbox editor only
    //[HideInInspector]
    public string[] hitBoxNames;

    public FightAnimationPlayableAsset(AnimationClip clip)
    {
        this.clip = clip;
    }

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<FightAnimationPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.Initialize(playable, graph, clip, hitBoxData);

        return playable;
    }

}