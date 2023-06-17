using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class FightAnimationPlayableAsset : PlayableAsset
{

    public AnimationClip clip;

    //will probably be public in the future
    private HitBoxPlayable hitBoxData;

    public FightAnimationPlayableAsset(AnimationClip clip)
    {
        this.clip = clip;
    }

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<FightAnimationPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.Initialize(playable, graph, clip);

        return playable;
    }

}

//make a new scene that serves as a hitbox editor
//create an editor script with a button on it that will call AssetDatabase.CreateAsset and create a playable file
//have it save hitbox data based on the scene