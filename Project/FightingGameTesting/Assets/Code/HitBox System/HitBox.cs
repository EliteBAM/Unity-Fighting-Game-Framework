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
        transform.localScale = new Vector3(size.x, size.y, 0);
    }

    private void OnDrawGizmos()
    {
        DrawHitboxGizmos();
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
