using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothDragController : MonoBehaviour
{
    public Transform target;


    public float rotateSpeed = 5f;
    public float speedFalloff = 1f;

    private float initialRotationY;
    private Vector3 lastMousePosition;
    private float rotationSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        initialRotationY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            rotationSpeed = mouseDelta.x * rotateSpeed * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            rotationSpeed *= speedFalloff;
        }

        Quaternion rotation = Quaternion.Euler(0f, initialRotationY + rotationSpeed, 0f);
        transform.RotateAround(new Vector3(target.position.x, transform.position.y + 20, target.position.z), Vector3.up, rotationSpeed);
        transform.LookAt(target.position);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
