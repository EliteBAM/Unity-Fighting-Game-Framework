using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateManager
{
    public Queue stateQueue;

    public State currentState;

    // reference to the player's animation manager to execute moves
    private AnimationManager animationManager;


    public StateManager(AnimationManager animationManager)
    {
        this.animationManager = animationManager;

        stateQueue = new Queue();

        currentState = new State(null, null, StateType.Idle);
    }

    public void UpdateCurrentState()
    {
        if ((animationManager.GetAnimationSwitcher().GetRemainingTime() <= 0 || currentState.stateType == StateType.Idle) && stateQueue.Count > 0)
        {
            //pop the next state off the queue and run it's animation
            Debug.Log("It's time for a new player state!");


            State nextState = (State) stateQueue.Dequeue();
            animationManager.PlayAnimation(nextState.animationName);
            currentState = nextState;

        }
        else if(animationManager.GetAnimationSwitcher().GetRemainingTime() <= 0 && stateQueue.Count == 0)
        {
            currentState = new State(null, null, StateType.Idle);
        }
    }

    public void AddStateToQueue(string animationName, string hitAnim, StateType stateType)
    {
        stateQueue.Enqueue(new State(animationName, hitAnim, stateType));
    }

}

[System.Serializable]
public class State
{

    public StateType stateType;
    public string animationName;
    public string hitAnim;


    public State(string animationName, string hitAnim, StateType stateType)
    {
        this.stateType = stateType;
        this.animationName = animationName;
        this.hitAnim = hitAnim;
    }
}

public enum StateType : byte
{
    Idle,
    Walking,
    Combo,
    Special
}
