using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class FightAnimationPlayable : PlayableBehaviour
{

    public ScriptPlayable<FightAnimationPlayable> owner;

    public AnimationClipPlayable clipPlayable;

    public ScriptPlayable<HitBoxPlayable> hitBoxDataPlayable;
    public HitBoxPlayable hitBoxDataBehaviour;

    public void Initialize(Playable owner, PlayableGraph graph, AnimationClip clip, HitBoxData[] data)
    {
        this.owner = (ScriptPlayable<FightAnimationPlayable>)owner;
        owner.SetInputCount(2);
        owner.SetOutputCount(2);

        clipPlayable = AnimationClipPlayable.Create(graph, clip);

        hitBoxDataPlayable = ScriptPlayable<HitBoxPlayable>.Create(graph);
        hitBoxDataBehaviour = hitBoxDataPlayable.GetBehaviour();
        hitBoxDataBehaviour.Initialize(data, clip.length);

        graph.Connect(clipPlayable, 0, owner, 0);
        graph.Connect(hitBoxDataPlayable, 0, owner, 1);

    }

    public void ResetTime()
    {
        owner.SetTime(0 - Time.deltaTime);
        owner.SetTime(0);

        clipPlayable.SetTime(0 - Time.deltaTime);
        clipPlayable.SetTime(0);

        hitBoxDataPlayable.SetTime(0 - Time.deltaTime);
        clipPlayable.SetTime(0);
    }

    public double SetDuration(double duration)
    {
        owner.SetDuration(duration);
        clipPlayable.SetDuration(duration);
        hitBoxDataPlayable.SetDuration(duration);

        return duration;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        clipPlayable.Play();
        hitBoxDataPlayable.Play();
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        clipPlayable.Pause();
        hitBoxDataPlayable.Pause();
    }

}
