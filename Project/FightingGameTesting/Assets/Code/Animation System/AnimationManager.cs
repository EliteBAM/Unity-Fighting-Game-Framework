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

    [SerializeField] FightAnimationPlayableAsset[] fightAnimations;


    public AnimationManager(Animator animator, HitBoxAnimator hitBoxAnimator, CharacterDataSO data)
    {

        //Get FightAnimation Playables
        fightAnimations = new FightAnimationPlayableAsset[data.moveList.Length + data.basicAnimations.Length];
        Debug.LogWarning("clips length: "  + fightAnimations.Length);
        for (int i = 0; i < data.basicAnimations.Length; i++)
        {
            fightAnimations[i] = data.basicAnimations[i];
        }
        for (int i = 0; i < data.moveList.Length; i++)
        {
            fightAnimations[i + data.basicAnimations.Length] = data.moveList[i].moveAnim;
            Debug.LogWarning(fightAnimations[i].clip.name);
        }

        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);


        var animationSwitcherPlayable = ScriptPlayable<AnimationSwitcherPlayable>.Create(graph);
        animationSwitcherBehaviour = animationSwitcherPlayable.GetBehaviour();


        var animationPlayableOutput = AnimationPlayableOutput.Create(graph, "Animation Output", animator);
        var scriptPlayableOutput = ScriptPlayableOutput.Create(graph, "Hitbox Output");

        animationPlayableOutput.SetSourcePlayable(animationSwitcherPlayable, 0);
        scriptPlayableOutput.SetSourcePlayable(animationSwitcherPlayable, 1);
        scriptPlayableOutput.SetUserData(hitBoxAnimator);

        //must be initialized after creation of scriptPlayableOutput in order to capture non-null hitBoxAnimator reference
        animationSwitcherBehaviour.Initialize(animationSwitcherPlayable, graph, fightAnimations);


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
