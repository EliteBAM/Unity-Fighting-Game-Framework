using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField]
    private CharacterController p1;
    [SerializeField]
    private CharacterController p2;

    [SerializeField]
    public Animator p1Anim;
    [SerializeField]
    public Animator p2Anim;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        p2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //P1_UpdateWalkingAnimations();
        P2_UpdateWalkingAnimations();
    }

    void P1_UpdateWalkingAnimations()
    {
        //Move Right
        if(p1.movementManager.moveLeft)
        {
            p1Anim.SetBool("isWalkingBackward", true);
        }
        else
        {
            p1Anim.SetBool("isWalkingBackward", false);
        }
        //Move Left
        if(p1.movementManager.moveRight)
        {
            p1Anim.SetBool("isWalkingForward", true);
        }
        else
        {
            p1Anim.SetBool("isWalkingForward", false);
        }
    }

    void P2_UpdateWalkingAnimations()
    {
        //Move Right
        if (p2.movementManager.moveRight)
        {
            p2Anim.SetBool("isWalkingBackward", true);
        }
        else
        {
            p2Anim.SetBool("isWalkingBackward", false);
        }
        //Move Left
        if (p2.movementManager.moveLeft)
        {
            p2Anim.SetBool("isWalkingForward", true);
        }
        else
        {
            p2Anim.SetBool("isWalkingForward", false);
        }
    }
}
