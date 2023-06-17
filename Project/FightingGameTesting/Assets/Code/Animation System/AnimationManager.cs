using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[System.Serializable]
public class AnimationManager
{

    PlayableGraph graph;
    AnimationSwitcherPlayable animationSwitcherBehaviour;

    [SerializeField] FightAnimationPlayableAsset[] clips;

    public AnimationManager(Animator animator, CharacterDataSO data)
    {

        //Get FightAnimation Playables
        clips = new FightAnimationPlayableAsset[data.moveList.Length + data.basicAnimations.Length];
        Debug.LogWarning("clips length: "  + clips.Length);
        for (int i = 0; i < data.basicAnimations.Length; i++)
        {
            clips[i] = data.basicAnimations[i];
        }
        for (int i = 0; i < data.moveList.Length; i++)
        {
            clips[i + data.basicAnimations.Length] = data.moveList[i].moveAnim;
            Debug.LogWarning(clips[i].clip.name);
        }

        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);


        var animationSwitcherPlayable = ScriptPlayable<AnimationSwitcherPlayable>.Create(graph);
        animationSwitcherBehaviour = animationSwitcherPlayable.GetBehaviour();

        animationSwitcherBehaviour.Initialize(animationSwitcherPlayable, graph, clips);

        var animationPlayableOutput = AnimationPlayableOutput.Create(graph, "Animation Output", animator);
        var scriptPlayableOutput = ScriptPlayableOutput.Create(graph, "Hitbox Output");

        animationPlayableOutput.SetSourcePlayable(animationSwitcherPlayable, 0);
        scriptPlayableOutput.SetSourcePlayable(animationSwitcherPlayable, 1);

        graph.Play();

        GraphVisualizerClient.Show(graph);

        Debug.Log("Animation Manager initialized");

    }

    public void UpdateAnimationGraph()
    {
        if(graph.GetTimeUpdateMode() == DirectorUpdateMode.Manual)
            graph.Evaluate(0f);
    }

    public void DisplayAnimationGraph()
    {
        GraphVisualizerClient.ClearGraphs();
        GraphVisualizerClient.Show(graph);
    }

    public void DestroyAnimationGraph()
    {
        graph.Destroy();
    }

    public void PlayAnimation(string animationName)
    {
        animationSwitcherBehaviour.PlayAnimation(animationName);
    }

}
