using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Create New Character")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Name")]
    public CharacterRoster characterName;

    [Header("Character Stats")]
    public int jumpCount = 1;
    public float acceleration = 300f;
    public float maxSpeed = 600f;
    public float jumpForce = 70f;

    [Header("Movement Animations")]
    [SerializeField]
    public FightAnimationPlayableAsset[] basicAnimations;

    [Header("Move List")]
    [SerializeField]
    public Move[] moveList;

}

[System.Serializable]
public class Move
{
    public string moveName;
    public FightAnimationPlayableAsset moveAnim;
    public List<SequenceBlock> inputSequence;
}
