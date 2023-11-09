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
    float duration;

    public override void OnPlayableCreate(Playable playable)
    {
        animator = (HitBoxAnimator)playable.GetGraph().GetOutput(1).GetUserData();
        //Animator is initialized in fightPlayable (parent playable) initialization call
    }

    public void Initialize(HitBoxData[] data, float duration)
    {
        //do some stuff
        //initialize the hitbox data repository type from the hitbox data parameter data
        this.data = data;
        this.duration = duration;

        //load all hitbox objects into the scene onto the character
        animator.AddHitBoxes(data.Length);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        for (int i = 0; i < data.Length; i++)
        {
            animator.ChangeBoxType(data[i], i);
        }
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {

        //update the properties of each hitbox in the data type based on the frame info (ex. what frame we are on)
        //including...
        //update hitbox position (corner Vector2s)
        //update hit or hurt value
        //update enabled or disabled status

        // Assuming a frame rate of 30 fps for the animation clip
        // This can change based on your actual clip's frame rate.
        const float frameRate = 60f;
        
        //note to self: playable.GetDuration() does not get the animation clip duration, it returns some obscenely large number, dont know what it represents. Max duration maybe?
        double totalFrames = duration * frameRate;
        int currentFrame = (int)((playable.GetTime() - (int)(playable.GetTime() / duration) * duration) * frameRate); //THIS ON OCCASION PRODUCES FRAME -1. FIX THE MATH

        if (currentFrame < 0) //im a genius for this one
            currentFrame = 0;

        // Floor it to get the integer frame number
        //int frameNumber = Mathf.FloorToInt((float)currentFrame);

        //Debug.Log("Current Frame: " + frameNumber);

        for (int i = 0; i < data.Length; i++)
        {
            Debug.Log("so the hitbox playable is running the prepare frame loop: " + i);
            Debug.Log("MFrame: " + currentFrame);
            animator.UpdateHitBox(data[i], currentFrame, i);
        }


    }
}
