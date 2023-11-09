using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitBox : MonoBehaviour
{
    //references
    CharacterController myCharacter;

    //components
    public BoxCollider collider;

    //frame data
    public bool isEnabled = true;
    public HitBoxType type;
    public Vector3 center;
    public Vector3 size;

    void Start()
    {
        myCharacter = gameObject.transform.parent.gameObject.GetComponent<CharacterController>();
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localPosition = new Vector3(0, center.y, center.x);
        transform.localScale = new Vector3(size.x, size.y, 5);       //TEMPORARILY MADE HITBOXES SUPER WIDE
    }

    private void OnDrawGizmos()
    {
        DrawHitboxGizmos();
    }

    void OnTriggerEnter(Collider collision)
    {
        //thanks to collision layers hitboxes will only interact with enemy hitboxes, so no need to check for same-team collisions
        if(type == HitBoxType.Hurt && collision.gameObject.tag == HitBoxType.Hit.ToString())
        {
            //somehow, in here, compute the hit.
            //play animation, take damage, update all systems, etc. How do I get out of here?
            Debug.Log(LayerMask.LayerToName(gameObject.layer) + " got hit!");
            myCharacter.animationManager.PlayAnimation(collision.GetComponent<HitBox>().myCharacter.stateManager.currentState.hitAnim); //.PlayAnimation(myCharacter.)

            //need a way to get the state of the attacking player, in other words, need to know what move is currently active in order to play the appropriate reaction animation.
            //thoughts:
            //what about moves with multiple hits in its sequence? (are these called strings? Probably)
            //in that case, a move may have numerous hit reaction animations corresponding to it's animation. fuck
            //in that case, more specific deliniation is required; more than just knowing what move is playing, we need to know what "part" of the move is playing.

            //potential solutions:
            // further modify the Move data structure? Turn the hit anim variable into a list.
            //Oh fuck.. do we need to create subclasses that inherit move? Oh fuck... create a string subclass and a special move subclass.
                    //wait... maybe not. I was gonna say we should do this because special moves have only one hit, but that's actually not true-- a special move may look a lot..
                        //..like a string combo

            //or: oh shit!!! Instead of each move holding the hit reaction data, each hitbox in a move could have its own unique hit animation.
                //-- even more developed-- The Move structure will hold all the animations in its structure so that they can all be uploaded to the animation graph
                //but-- the HitBoxData data structure can be updated to hold a key framable index to one of those hit animations!!
                    //so when the hitbox hits something, it can call the index from the move structure and play it-- I just realized this won't work. fuck.

            //ok, so we dont need to create a subclass of move. Instead, we need to create a new class called String, which is a collection of moves that play in sequence.
                //wait
        }

        if(type == HitBoxType.Hit)
        {
            if (collision.gameObject.GetComponent<HitBox>().type == HitBoxType.Hit)
            {

            }
        }
    }

    void DrawHitboxGizmos()
    {
        if(collider.enabled)
        {
            Vector3 gizmoSize = size * transform.parent.transform.lossyScale.x;

            if (isEnabled)
            {
                if (type == HitBoxType.Hit)
                    Gizmos.color = Color.red;
                else if (type == HitBoxType.Hurt)
                    Gizmos.color = Color.green;

                // Draw the wireframe cube
                Gizmos.DrawWireCube(transform.position, gizmoSize);

                // Draw the solid cube
                Gizmos.DrawCube(transform.position, gizmoSize);
            }
        }
    }

}
