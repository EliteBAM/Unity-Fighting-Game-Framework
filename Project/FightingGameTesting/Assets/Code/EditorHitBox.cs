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

    Vector3 center;
    Vector3 size;

    void Start()
    {
        previousPosition = transform.position;

        collider = GetComponent<BoxCollider>();
        transform.GetComponent<MeshRenderer>().enabled = false;

        InitializeHandles();
    }

    // Update is called once per frame
    void Update()
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
            // Calculate the midpoint between two handles
            Vector3 midpoint = (handle1.transform.position + handle2.transform.position) / 2;

            // Calculate the dimensions of the box based on the handle positions
            Vector3 size = new Vector3(
                Mathf.Abs(handle2.transform.position.x - handle1.transform.position.x),
                Mathf.Abs(handle2.transform.position.y - handle1.transform.position.y),
                Mathf.Abs(handle2.transform.position.z - handle1.transform.position.z)
            );

            // Set gizmo color
            if (HitBoxEditorManager.instance.hitBoxData[index].boxType == HitBoxType.Hit)
                Gizmos.color = new Color(1, 0, 0, 0.5f);
            else
                Gizmos.color = new Color(0, 1, 0, 0.5f);

            // Draw the wireframe cube
            Gizmos.DrawWireCube(midpoint, new Vector3(size.x, size.y, size.z));

            // Draw the solid cube
            Gizmos.DrawCube(midpoint, new Vector3(size.x, size.y, size.z));
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
                center = HitBoxEditorManager.instance.hitBoxData[this.index].sizeChangeFrameData[index].Center;
                size = HitBoxEditorManager.instance.hitBoxData[this.index].sizeChangeFrameData[index].Size;

                transform.position = center;
                transform.localScale = size;

                handle1.transform.position = transform.position + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
                handle2.transform.position = transform.position + new Vector3(transform.localScale.x / 2, transform.localScale.y / 2);
            }
        }
    }

    void UpdateHandlePositions()
    {
        if(handlesVisible)
        {
            previousPosition = transform.position;

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
            }

            Vector3 handle1Position = handle1.transform.position;
            Vector3 handle2Position = handle2.transform.position;


            size = new Vector3(
                Mathf.Abs(handle2.transform.position.x - handle1.transform.position.x),
                Mathf.Abs(handle2.transform.position.y - handle1.transform.position.y),
                Mathf.Abs(handle2.transform.position.z - handle1.transform.position.z)
            );

            center = (handle1Position + handle2Position) / 2;

            transform.position = center;
            transform.localScale = size;

            handle1.transform.position = handle1Position;
            handle2.transform.position = handle2Position;

            if (transform.position != previousPosition)
                HitBoxEditorManager.instance.hitBoxData[index] = HitBoxEditorManager.instance.AppendSizeData(index, transform.position, size);

        }
    }

    private void InitializeHandles()
    {
        handle1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MeshRenderer handle1Renderer = handle1.GetComponent<MeshRenderer>();
        handle1Renderer.material = new Material(Shader.Find("Unlit/Color"));
        handle1Renderer.material.renderQueue = 4001;
        handle1Renderer.sharedMaterial.color = Color.green;
        handle1.transform.localScale = Vector3.one * 3;


        handle2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MeshRenderer handle2Renderer = handle2.GetComponent<MeshRenderer>();
        handle2Renderer.material = new Material(Shader.Find("Unlit/Color"));
        handle2Renderer.material.renderQueue = 4001;
        handle2Renderer.sharedMaterial.color = Color.green;
        handle2.transform.localScale = Vector3.one * 3;


        handle1.tag = "handle";
        handle2.tag = "handle";

        handle1.layer = 6;
        handle2.layer = 6;

        handle1.transform.position = transform.position + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
        handle2.transform.position = transform.position + new Vector3(transform.localScale.x / 2, transform.localScale.y / 2);

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
