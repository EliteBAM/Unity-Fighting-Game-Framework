using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{

    public GameObject target;
    public GameObject previousTarget;

    [Header("Options")]
    public bool followPosition = true;
    public bool followRotation = false;

    [Header("Settings")]
    public float positionFollowSpeed = 2f;
    public float rotationFollowSpeed = 2f;

    Vector3 velocity = Vector3.zero;


    private void Start()
    {
        transform.position = target.transform.position; //putting this line in start stops camera stutter on start in editor
        previousTarget = target;
    }

    private void Update()
    {
        if (target == null)
            target = previousTarget;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
            target = previousTarget;

        if (followPosition)
            transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, positionFollowSpeed);

        if(followRotation)
            transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotationFollowSpeed * Time.deltaTime);
    }

    public void SwitchTarget(GameObject target)
    {
        previousTarget = previousTarget = this.target ?? previousTarget;
        this.target = target;
    }
}
