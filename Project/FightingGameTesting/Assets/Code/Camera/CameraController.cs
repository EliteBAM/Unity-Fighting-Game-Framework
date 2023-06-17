using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject minX;
    public GameObject maxX;
    public GameObject minZ;
    public GameObject maxZ;
    public GameObject distanceCenter;

    public GameObject player1;
    public GameObject player2;

    //Player Orientation Variables
    public static bool flipP1 = false;
    public static bool flipP2 = false;

    //Z Variables
    public float zDistanceModifier = 3f;
    public float zDistanceMultiplier;

    //X Variables
    public float xPlayerMidpoint;
    public float rawPlayerDistance;
    public static float playerDistance;
    public static int maxPlayerDistance = 20;

    //Y Variables
    public float yPlayerMidpoint;
    public float yOffset = 2.5f;


    public void Awake()
    {
        //Must be pre-calculated so that players don't flip on start-up
        CalculatePlayerDistance();

        //Calling the updates in awake helps eliminate camera stutter on start-up in editor
        UpdateZPos();
        UpdateXPos();
        UpdateYPos();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerFlip();
        CalculatePlayerDistance();

        UpdateZPos();
        UpdateXPos();
        UpdateYPos();
    }

    void UpdateYPos()
    {
        yPlayerMidpoint = (player1.transform.position.y + player2.transform.position.y) / 2;
        distanceCenter.transform.position = new Vector3(distanceCenter.transform.position.x, yPlayerMidpoint + yOffset, distanceCenter.transform.position.z);

        transform.position = new Vector3(transform.position.x, yPlayerMidpoint + yOffset, transform.position.z);
    }

    void UpdateXPos()
    {

        xPlayerMidpoint = (player1.transform.position.x + player2.transform.position.x) / 2;
        distanceCenter.transform.position = new Vector3(xPlayerMidpoint, distanceCenter.transform.position.y, distanceCenter.transform.position.z);

        if(xPlayerMidpoint < minX.transform.position.x)
        {
            xPlayerMidpoint = minX.transform.position.x;
        }
        if (xPlayerMidpoint > maxX.transform.position.x)
        {
            xPlayerMidpoint = maxX.transform.position.x;
        }

        transform.position = new Vector3(xPlayerMidpoint, transform.position.y, transform.position.z);

    }

    void UpdateZPos()
    {
        zDistanceMultiplier = playerDistance * zDistanceModifier;

        transform.position = new Vector3(transform.position.x, transform.position.y, zDistanceMultiplier);

        //Debug.Log(transform.position.z + ", " + minZ.transform.position.z + ", " + maxZ.transform.position.z);                              

        if (transform.position.z > minZ.transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minZ.transform.position.z);
        }
        if (transform.position.z < maxZ.transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ.transform.position.z);
        }

        //Debug.Log(transform.position.z + ", " + minZ.transform.position.z);
    }

    public void CalculatePlayerDistance()
    {
        rawPlayerDistance = player1.transform.position.x - player2.transform.position.x;
        playerDistance = Mathf.Abs(rawPlayerDistance);
    }

    public void CheckPlayerFlip()
    {
        if(Mathf.Sign(player1.transform.position.x - player2.transform.position.x) != Mathf.Sign(rawPlayerDistance))
        {
            flipP1 = true;
            flipP2 = true;
        }
    }

}
