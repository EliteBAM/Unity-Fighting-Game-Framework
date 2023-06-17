using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AnimationSwitcherPlayable : PlayableBehaviour
{

    (ScriptPlayable<FightAnimationPlayable> Playable, int Index) currentClip;
    double clipDuration = 0.0f;

    Playable mixer;


    public void Initialize(Playable owner, PlayableGraph graph, FightAnimationPlayableAsset[] fightAnimations)
    {

        owner.SetInputCount(1);
        owner.SetOutputCount(2);

        mixer = AnimationMixerPlayable.Create(graph, fightAnimations.Length);

        graph.Connect(mixer, 0, owner, 0);

        owner.SetInputWeight(0, 1);


        for (int i = 0; i < mixer.GetInputCount(); i++)
        {
            var fightAnimation = fightAnimations[i].CreatePlayable(graph, null);
            
            graph.Connect(fightAnimation, 0, mixer, i);

            //graph.Connect(AnimationClipPlayable.Create(graph, clips[i]), 0, mixer, i);

            mixer.SetInputWeight(i, 0.0f);
            mixer.GetInput(i).Pause();
        }

        var fightPlayable = (ScriptPlayable<FightAnimationPlayable>)mixer.GetInput(0);
        currentClip = (fightPlayable, 0);
        mixer.SetInputWeight(0, 1.0f);

        currentClip.Playable.Play();
    
    }

    public void PlayAnimation(string animationName)
    {
        for(int i = 0; i < mixer.GetInputCount(); i++)
        {
            var fightPlayable = (ScriptPlayable<FightAnimationPlayable>)mixer.GetInput(i);
            var behaviour = fightPlayable.GetBehaviour();

            if(behaviour.clipPlayable.GetAnimationClip().name == animationName)
            {
                mixer.SetInputWeight(currentClip.Index, 0.0f); //turn off current animation
                currentClip.Playable.Pause();

                currentClip = (fightPlayable, i); //set new animation to current clip

                behaviour.ResetTime();

                mixer.SetInputWeight(currentClip.Index, 1.0f); //turn on new current animation

                currentClip.Playable.Play();

                clipDuration = currentClip.Playable.GetBehaviour().clipPlayable.GetAnimationClip().length;
                Debug.Log("clip duration: " + clipDuration);


                Debug.Log("animation was played");
            }
        }
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {

        ResetToIdle();

        if (clipDuration > 0.0f)
            clipDuration -= info.deltaTime;

    }

    private void ResetToIdle()
    {
        //this block defaults character to idle animation when other non-looping animations are finished playing
        if (!currentClip.Playable.GetBehaviour().clipPlayable.GetAnimationClip().isLooping && clipDuration <= 0)
        {
            //reset and pause old clip
            mixer.SetInputWeight(currentClip.Index, 0);
            currentClip.Playable.SetTime(0);
            currentClip.Playable.Pause();

            //reset and play new clip
            var fightPlayable = (ScriptPlayable<FightAnimationPlayable>)mixer.GetInput(0);
            currentClip = (fightPlayable, 0);

            currentClip.Playable.SetTime(0);
            mixer.SetInputWeight(0, 1);
            currentClip.Playable.Play();


            Debug.Log("animation was reset to idle");
            return;
        }
    }
}
