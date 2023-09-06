using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameHitBox : MonoBehaviour
{
    public bool isEnabled = true;


    private BoxCollider collider;


    public Vector3 center;
    public Vector3 size;

    void Start()
    {

        //collider = GetComponent<BoxCollider>();
        //transform.GetComponent<MeshRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
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
        if (isEnabled)
        {
            // Draw the wireframe cube
            Gizmos.DrawWireCube(center, size);

            // Draw the solid cube
            Gizmos.DrawCube(center, size);
        }
    }

}
