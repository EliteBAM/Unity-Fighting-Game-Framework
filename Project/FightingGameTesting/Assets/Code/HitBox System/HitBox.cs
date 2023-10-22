using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitBox : MonoBehaviour
{
    public bool isEnabled = true;


    public BoxCollider collider;

    public HitBoxType type;

    public Vector3 center;
    public Vector3 size;

    void Start()
    {
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
        Debug.Log("we're colliding with something");
        //thanks to collision layers hitboxes will only interact with enemy hitboxes, so no need to check for same-team collisions
        if(type == HitBoxType.Hurt && collision.gameObject.tag == HitBoxType.Hit.ToString())
        {
            //somehow, in here, compute the hit.
            //play animation, take damage, update all systems, etc. How do I get out of here?
            Debug.Log("someone got hit!");
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
