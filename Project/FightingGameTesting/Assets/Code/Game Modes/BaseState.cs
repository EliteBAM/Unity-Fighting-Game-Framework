using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New State", menuName = "State Machine/States")]
public class BaseState : ScriptableObject
{

    [Header("State Transitions")]

    [Header("State Functionality")]
    public CutSceneStateBehaviour stateBehaviour;

    //Spublic abstract EState Transition();

}
