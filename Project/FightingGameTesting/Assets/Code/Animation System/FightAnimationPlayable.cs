using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class FightAnimationPlayable : PlayableBehaviour
{

    public ScriptPlayable<FightAnimationPlayable> owner;

    public AnimationClipPlayable clipPlayable;

    public ScriptPlayable<HitBoxPlayable> hitBoxData;

    public void Initialize(Playable owner, PlayableGraph graph, AnimationClip clip)
    {
        this.owner = (ScriptPlayable<FightAnimationPlayable>)owner;
        owner.SetInputCount(2);
        owner.SetOutputCount(2);

        clipPlayable = AnimationClipPlayable.Create(graph, clip);

        hitBoxData = ScriptPlayable<HitBoxPlayable>.Create(graph);

        graph.Connect(clipPlayable, 0, owner, 0);
        graph.Connect(hitBoxData, 0, owner, 1);

    }

    public void ResetTime()
    {
        owner.SetTime(0 - Time.deltaTime);
        owner.SetTime(0);

        clipPlayable.SetTime(0 - Time.deltaTime);
        clipPlayable.SetTime(0);

        hitBoxData.SetTime(0 - Time.deltaTime);
        clipPlayable.SetTime(0);
    }

    public double SetDuration(double duration)
    {
        owner.SetDuration(duration);
        clipPlayable.SetDuration(duration);
        hitBoxData.SetDuration(duration);

        return duration;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        clipPlayable.Play();
        hitBoxData.Play();
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        clipPlayable.Pause();
        hitBoxData.Pause();
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {



    }
}
