using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public float swayFollowSpeed = 1;
    public float xSpeed = 1;
    public float ySpeed = 1;
    public float xIntensity = 1;
    public float yIntensity = 1;

    public float mouseInfluence = 2;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private float mouseXDelta;
    private float mouseYDelta;

    void Start()
    {
        initialRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x + Mathf.Cos(Time.time * ySpeed) * yIntensity, 
                                              initialRotation.eulerAngles.y + Mathf.Sin(Time.time * xSpeed) * xIntensity, 
                                              initialRotation.eulerAngles.z);

    }

    void LateUpdate()
    {
        SwayCamera();
    }

    void SwayCamera()
    {
        //mouse inputs
        mouseXDelta = Input.GetAxisRaw("Mouse X");
        mouseYDelta = Input.GetAxisRaw("Mouse Y");

        targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x + Mathf.Cos(Time.time * ySpeed) * yIntensity - mouseYDelta * mouseInfluence,
                                          initialRotation.eulerAngles.y + Mathf.Sin(Time.time * xSpeed) * xIntensity + mouseXDelta * mouseInfluence, 
                                          initialRotation.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, swayFollowSpeed * Time.deltaTime);
    }

    void CameraBurst()
    {

    }
}
