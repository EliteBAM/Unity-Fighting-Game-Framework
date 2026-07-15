#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorHitBox : MonoBehaviour
{
    public int index;

    public bool isSelected = false;
    public bool isEnabled = true;

    private bool handlesVisible = false;

    private BoxCollider collider;

    public GameObject handle1;
    public GameObject handle2;

    private Vector3 previousPosition;
    Transform character;

    public Vector2 center;
    public Vector2 size;

    void Start()
    {
        previousPosition = transform.localPosition;
        character = transform.parent.parent;
        transform.localScale = new Vector3(transform.localScale.x / character.localScale.x,
                                           transform.localScale.y / character.localScale.y,
                                           transform.localScale.z / character.localScale.z);


        InitializeHandles();
        CalculateSizeFromHandles();

        transform.localPosition = new Vector3(0, center.y, center.x);
        transform.localScale = new Vector3(size.x, size.y, 0);

        collider = GetComponent<BoxCollider>();
        transform.GetComponent<MeshRenderer>().enabled = false;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        ShowHandles();

        UpdateEnabled();
        GetPositionInFrame();
        UpdateHandlePositions();
    }

    private void OnDrawGizmos()
    {
        DrawHitboxGizmos();
    }

    void DrawHitboxGizmos()
    {
        if(isEnabled)
        {
            // Set gizmo color
            if (HitBoxEditorManager.instance.hitBoxData[index].boxType == HitBoxType.Hit)
                Gizmos.color = new Color(1, 0, 0, 0.5f);
            else
                Gizmos.color = new Color(0, 1, 0, 0.5f);

            Vector2 worldSpaceSize = new Vector2(
                Mathf.Abs((handle2.transform.localPosition.z - handle1.transform.localPosition.z) * character.localScale.z),
                Mathf.Abs((handle2.transform.localPosition.y - handle1.transform.localPosition.y) * character.localScale.y)
            );

            Debug.Log("DRAWING GIZMOS: SIZE X: " + worldSpaceSize.x + " SIZE Y: " + worldSpaceSize.y);
            // Draw the wireframe cube
            Gizmos.DrawWireCube(transform.position, new Vector3(worldSpaceSize.x, worldSpaceSize.y, 0));

            // Draw the solid cube
            Gizmos.DrawCube(transform.position, new Vector3(worldSpaceSize.x, worldSpaceSize.y, 0));
        }
    }


    private bool isDragging = false;
    private Vector3 lastMousePosition;

    public float dragSpeed = 1.0f;
    RaycastHit hit;

    void UpdateEnabled()
    {
        if (HitBoxEditorManager.instance.currentFrame >= HitBoxEditorManager.instance.hitBoxData[index].startFrame && HitBoxEditorManager.instance.currentFrame <= HitBoxEditorManager.instance.hitBoxData[index].endFrame)
        {
            if(!isEnabled)
            {
                Debug.Log("Enabling hitbox");
                collider.enabled = true;
                isEnabled = true;
                if(isSelected)
                {
                    handle1.SetActive(true);
                    handle2.SetActive(true);
                }
            }
        }
        else if(isEnabled)
        {
            Debug.Log("Disabling hitbox");
            collider.enabled = false;
            isEnabled = false;
            if(isSelected)
            {
                handle1.SetActive(false);
                handle2.SetActive(false);
            }
        }
    }

    void GetPositionInFrame()
    {
        if(gameObject.activeSelf)
        {
            int frame = HitBoxEditorManager.instance.currentFrame + 1;

            int index;

            do
            {
                index = Array.IndexOf(HitBoxEditorManager.instance.hitBoxData[this.index].sizeChangeFrames, --frame);
            } while (index == -1 && frame > 0);

            if (index != -1)
            {
                center = HitBoxEditorManager.instance.hitBoxData[this.index].center[index];
                size = HitBoxEditorManager.instance.hitBoxData[this.index].size[index];

                Debug.Log("Getting position for frame: " + center);
                transform.localPosition = new Vector3(0, center.y, center.x);
                transform.localScale = new Vector3(size.x, size.y, 0);

                handle1.transform.localPosition = transform.localPosition + new Vector3(0, -transform.localScale.y / 2, -transform.localScale.x / 2);
                handle2.transform.localPosition = transform.localPosition + new Vector3(0, transform.localScale.y / 2, transform.localScale.x / 2);
            }
        }
    }

    void UpdateHandlePositions()
    {
        if(handlesVisible)
        {
            // If left mouse button is pressed, try to start dragging
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit) && (hit.transform == handle1.transform || hit.transform == handle2.transform))
                {
                    isDragging = true;
                    lastMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
                }
            }

            // If left mouse button is released, stop dragging
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // If currently dragging, update the position of the object based on mouse delta
            if (isDragging)
            {
                Vector3 currentMouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
                Vector3 difference = currentMouseWorldPosition - lastMousePosition;

                hit.transform.Translate(difference.x * dragSpeed, difference.y * dragSpeed, 0, Space.Self);

                lastMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
                SetSize();
            }

        }
    }

    public void CalculateSizeFromHandles()
    {
        //Calculate size after handle adjustment
        size = new Vector2(
            Mathf.Abs(handle2.transform.localPosition.z - handle1.transform.localPosition.z),
            Mathf.Abs(handle2.transform.localPosition.y - handle1.transform.localPosition.y)
        );

        //calculate center after handle adjustment
        Vector2 handle1_ZY_LocalPos = new Vector2(handle1.transform.localPosition.z, handle1.transform.localPosition.y);
        Vector2 handle2_ZY_LocalPos = new Vector2(handle2.transform.localPosition.z, handle2.transform.localPosition.y);
        center = (handle1_ZY_LocalPos + handle2_ZY_LocalPos) / 2;
    }

    public void SetSize()
    {
        previousPosition = transform.localPosition;

        CalculateSizeFromHandles();

        //Set new center and size
        transform.localPosition = new Vector3(0, center.y, center.x);
        transform.localScale = new Vector3(size.x, size.y, 0);

        if (transform.localPosition != previousPosition)
            HitBoxEditorManager.instance.hitBoxData[index] = 
                HitBoxEditorManager.instance.AppendSizeData(index, center, size);
    }

    private void InitializeHandles()
    {
        handle1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MeshRenderer handle1Renderer = handle1.GetComponent<MeshRenderer>();
        handle1Renderer.material = new Material(Shader.Find("Unlit/Color"));
        handle1Renderer.material.renderQueue = 4001;
        handle1Renderer.sharedMaterial.color = Color.green;
        handle1.transform.localScale = Vector3.one * 3;
        handle1.transform.SetParent(transform.parent);


        handle2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MeshRenderer handle2Renderer = handle2.GetComponent<MeshRenderer>();
        handle2Renderer.material = new Material(Shader.Find("Unlit/Color"));
        handle2Renderer.material.renderQueue = 4001;
        handle2Renderer.sharedMaterial.color = Color.green;
        handle2.transform.localScale = Vector3.one * 3;
        handle2.transform.SetParent(transform.parent);


        handle1.tag = "handle";
        handle2.tag = "handle";

        handle1.layer = 6;
        handle2.layer = 6;

        handle1.transform.localPosition = transform.localPosition + new Vector3(0, -transform.localScale.y / 2, -transform.localScale.x / 2);
        handle2.transform.localPosition = transform.localPosition + new Vector3(0, transform.localScale.y / 2, transform.localScale.x / 2);

        handle1.SetActive(false);
        handle2.SetActive(false);
    }

    void ShowHandles()
    {
        if (isSelected)
        {
            if (!handlesVisible)
            {
                handle1.SetActive(true);
                handle2.SetActive(true);
                handlesVisible = true;
            }
        }
        else
        {
            if (handlesVisible)
            {
                handle1.SetActive(false);
                handle2.SetActive(false);
                handlesVisible = false;
            }
        }
    }
}
#endif