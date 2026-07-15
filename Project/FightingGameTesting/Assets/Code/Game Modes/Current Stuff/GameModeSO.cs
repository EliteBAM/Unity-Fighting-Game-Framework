using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Game Mode", menuName = "Create New Game Mode")]
public class GameModeSO : ScriptableObject
{

    public int rounds;

    public float roundTime;

    public bool tieBreaker;

}
