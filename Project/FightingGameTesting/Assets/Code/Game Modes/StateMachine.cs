using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [Header("State Machine Preset")]
    public StateMachineSO stateMachineData;

    public GameContext context;

    [Header("States")]
    public List<BaseState> States;


    [SerializeField]
    protected BaseState currentState;

    // Start is called before the first frame update
    void Start()
    {

        InitializeStateMachine();

        currentState.stateBehaviour.EnterState();
    }

    // Update is called once per frame
    void Update()
    {

        currentState.stateBehaviour.UpdateState();
        
    }

    void InitializeStateMachine()
    {

        //stateBehaviour = stateMachineData.state

        foreach(BaseState state in stateMachineData.States)
        {
            BaseState newState = state;
            newState.stateBehaviour.InitializeState();

            //if newState.SuccessfullyInitialized == true
                //add to States list
        }

        //create state instances from state data
        
        //initialize them by initializing their required context from the game context.
            //if it required context is missing, throw error and destroy state

        //add initialized states to the state list
        
        //Add transition logic to required area
    }

}
