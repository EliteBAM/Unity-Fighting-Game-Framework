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


    public AnimationManager(Animator animator, HitBoxAnimator hitBoxAnimator, CharacterDataSO data, CharacterDataSO enemyData)
    {

        //Get FightAnimation Playables
        if(enemyData != null)
            fightAnimations = new FightAnimationPlayableAsset[data.moveList.Length + data.basicAnimations.Length + enemyData.moveList.Length];
        else
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
        if(enemyData != null)
        {
            for (int i = 0; i < enemyData.moveList.Length; i++)
            {
                fightAnimations[i + data.basicAnimations.Length + data.moveList.Length] = enemyData.moveList[i].hitAnim;
            }
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

    public AnimationSwitcherPlayable GetAnimationSwitcher()
    {
        return animationSwitcherBehaviour;
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

//so far, what is the only way an animation is called onto a player? through the input manager, which calls the animation manager PlayAnimation function
//what does this mean? So far, there is no defined avenue for calling animations onto characters that were not directly inputted.
//There needs to be a defined avenue for calling reactionary animations, not just user-inputted movements.
//how can this system be integrated?
//Off the dome, another module can be added to the character controller, which like the input manager, will have a private reference to the animation managaer.
//this will be the reaction manager, it will call animations to the character that come from computer-computer interactions, including (but not limited to?) hitbox interactions
//okay, this sounds feasible, and although maybe not perfectly optimized, it appears to be an organized solution that will be easy to navigate and matches the initial design pattern
//notes on how this might be implemented: Animation manager is the currently designated object for feeding necessary animations to the animation graph--
//currently it only sources animations from the character move sheet. It seems reasonable given this object's purpose as tbe animation curator that it would be updated to..
//.. also source necessary non-input animations, such as hit reaction animations from the opposing player's moveset, as well as environmental reaction animations (future feature)
//more implementation notes:
//certain data structures would need to be updated:
// 1) Move structure looks like it needs to be updated to store reaction animation. (yesss, then I can copy the for loop that loops through your moves and gets your input animations, and make it loop through your enemy's moves and gets their reaction animations onto your player graph!)
// 2) The Animation Manager module may also need a reference to the HitBox Animator component, which contains each player's hitbox repository. This might be useful for getting collision data? Not sure yet if necessary
