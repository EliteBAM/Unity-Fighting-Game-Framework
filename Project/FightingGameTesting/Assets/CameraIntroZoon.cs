using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntroZoon : MonoBehaviour
{

    float finalPosition;
    float startPosition = 20f;

    float currentPosition;

    public float speed = 1f;

    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        finalPosition = transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, startPosition);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, finalPosition), ref velocity, speed);
    }
}
