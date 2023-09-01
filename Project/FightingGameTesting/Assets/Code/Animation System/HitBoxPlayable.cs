using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HitBoxPlayable : PlayableBehaviour
{

    //data type that stores hitboxes in the scene-- jk, just reference the HitBoxAnimator Monobehaviour,
    //...where this datastructure is stored in an environment that can interact with the sceneanim
    public HitBoxAnimator animator;

    HitBoxData[] data;

    public override void OnPlayableCreate(Playable playable)
    {
        animator = (HitBoxAnimator)playable.GetGraph().GetOutput(1).GetUserData();
        //Animator is initialized in fightPlayable (parent playable) initialization call
    }

    public void Initialize(HitBoxData[] data)
    {
        //do some stuff
        //initialize the hitbox data repository type from the hitbox data parameter data
        this.data = data;

        //load all hitbox objects into the scene onto the character
        animator.AddHitBoxes(data.Length);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {

        //update the properties of each hitbox in the data type based on the frame info (ex. what frame we are on)
        //including...
        //update hitbox position (corner Vector2s)
        //update hit or hurt value
        //update enabled or disabled status


    }
}
