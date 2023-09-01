using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HitBoxTesting : MonoBehaviour
{

    public Vector3 boxSize = new Vector3(5, 5, 5);

    public Collider[] intersectingColliders;

    public float thickness = 3;

    public float transparency = 0.2f;

    public bool hurtBox;
    public bool hitBox;

    //front face
    private Vector3 topStartPos1;
    private Vector3 topEndPos1;

    private Vector3 botStartPos1;
    private Vector3 botEndPos1;

    private Vector3 leftStartPos1;
    private Vector3 leftEndPos1;

    private Vector3 rightStartPos1;
    private Vector3 rightEndPos1;

    //back face
    private Vector3 topStartPos2;
    private Vector3 topEndPos2;

    private Vector3 botStartPos2;
    private Vector3 botEndPos2;

    private Vector3 leftStartPos2;
    private Vector3 leftEndPos2;

    private Vector3 rightStartPos2;
    private Vector3 rightEndPos2;

    //right face
    private Vector3 topStartPos3;
    private Vector3 topEndPos3;

    private Vector3 botStartPos3;
    private Vector3 botEndPos3;

    //left face
    private Vector3 topStartPos4;
    private Vector3 topEndPos4;

    private Vector3 botStartPos4;
    private Vector3 botEndPos4;

    Color color;

    public void Awake()
    {

    }

    private void Update()
    {
        intersectingColliders = Physics.OverlapBox(transform.position, boxSize / 2, transform.rotation);

        CheckHit();
    }

    private void OnDrawGizmos()
    {
        DisableTransformations();

        UpdateHitBoxSelection();

        DrawHitboxGizmos();
#if UNITY_EDITOR
        DrawEdges();
#endif
    }

    void CheckHit()
    {
        if (intersectingColliders.Length > 0)
        {
            Debug.Log("We hit something");
        }
    }

    void DisableTransformations()
    {
        if (transform.localScale.x != 1 || transform.localScale.y != 1 || transform.localScale.z != 1)
        {
            transform.localScale = Vector3.one;
        }

        if (transform.rotation.x != 0 || transform.rotation.y != 0 || transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void UpdateHitBoxSelection()
    {
        if (hitBox && !hurtBox)
        {
            if ((hitBox && hurtBox) || hitBox)
            {
                hurtBox = false;
                Gizmos.color = Color.red;
                color = Color.red;
            }
        }

        if (hurtBox)
        {
            if ((hurtBox && hitBox) || hurtBox)
            {
                hitBox = false;
                Gizmos.color = Color.green;
                color = Color.green;
            }
        }
    }

    void DrawHitboxGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxSize.x, boxSize.y, boxSize.z)); // Because size is halfExtents

        if (hitBox)
            Gizmos.color = new Color(1, 0, 0, transparency);
        if (hurtBox)
            Gizmos.color = new Color(0, 1, 0, transparency);

        Gizmos.DrawCube(Vector3.zero, new Vector3(boxSize.x, boxSize.y, boxSize.z)); // Because size is halfExtents

    }

#if UNITY_EDITOR
    void DrawEdges()
    {

        //front face
        topStartPos1 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z + boxSize.z / 2);
        topEndPos1 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z + boxSize.z / 2);

        botStartPos1 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z + boxSize.z / 2);
        botEndPos1 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z + boxSize.z / 2);

        leftStartPos1 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z + boxSize.z / 2);
        leftEndPos1 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z + boxSize.z / 2);

        rightStartPos1 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z + boxSize.z / 2);
        rightEndPos1 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z + boxSize.z / 2);

        //back face
        topStartPos2 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z - boxSize.z / 2);
        topEndPos2 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z - boxSize.z / 2);

        botStartPos2 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z - boxSize.z / 2);
        botEndPos2 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z - boxSize.z / 2);

        leftStartPos2 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z - boxSize.z / 2);
        leftEndPos2 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z - boxSize.z / 2);

        rightStartPos2 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z - boxSize.z / 2);
        rightEndPos2 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z - boxSize.z / 2);

        //right face
        topStartPos3 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z - boxSize.z / 2);
        topEndPos3 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z + boxSize.z / 2);

        botStartPos3 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z - boxSize.z / 2);
        botEndPos3 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z + boxSize.z / 2);

        //left face
        topStartPos4 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z - boxSize.z / 2);
        topEndPos4 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, transform.position.z + boxSize.z / 2);

        botStartPos4 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z - boxSize.z / 2);
        botEndPos4 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, transform.position.z + boxSize.z / 2);

        //front face
        Handles.DrawBezier(topStartPos1, topEndPos1, topStartPos1, topEndPos1, color, null, thickness);
        Handles.DrawBezier(botStartPos1, botEndPos1, botStartPos1, botEndPos1, color, null, thickness);
        Handles.DrawBezier(leftStartPos1, leftEndPos1, leftStartPos1, leftEndPos1, color, null, thickness);
        Handles.DrawBezier(rightStartPos1, rightEndPos1, rightStartPos1, rightEndPos1, color, null, thickness);

        //back face
        Handles.DrawBezier(topStartPos2, topEndPos2, topStartPos2, topEndPos2, color, null, thickness);
        Handles.DrawBezier(botStartPos2, botEndPos2, botStartPos2, botEndPos2, color, null, thickness);
        Handles.DrawBezier(leftStartPos2, leftEndPos2, leftStartPos2, leftEndPos2, color, null, thickness);
        Handles.DrawBezier(rightStartPos2, rightEndPos2, rightStartPos2, rightEndPos2, color, null, thickness);

        if (boxSize.z != 0)
        {
            //right face
            Handles.DrawBezier(topStartPos3, topEndPos3, topStartPos3, topEndPos3, color, null, thickness);
            Handles.DrawBezier(botStartPos3, botEndPos3, botStartPos3, botEndPos3, color, null, thickness);

            //left face
            Handles.DrawBezier(topStartPos4, topEndPos4, topStartPos4, topEndPos4, color, null, thickness);
            Handles.DrawBezier(botStartPos4, botEndPos4, botStartPos4, botEndPos4, color, null, thickness);
        }
    }
#endif


}
