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
