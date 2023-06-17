using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{

    public GameObject target;
    public float speed = 2f;

    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        transform.position = target.transform.position; //putting this line in start stops camera stutter on start in editor
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, speed);
    }
}
