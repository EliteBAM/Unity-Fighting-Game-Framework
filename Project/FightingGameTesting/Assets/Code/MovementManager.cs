using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementManager
{
    //general values shared between all characters
    public static float gravityMultiplier = 75f;

    public static float backStepDownscale = 0.75f; //reduces movement speed when moving in the backwards direction by this multiplier

    [Header("Movement Settings")]
    [SerializeField] public int maxJumpCount = 1;
    [SerializeField] public float acceleration = 300f;
    [SerializeField] public float maxSpeed = 600f;
    [SerializeField] public float jumpForce = 70f;

    //technical variables
    [HideInInspector] public float jumpCooldownTimer = 0f;
    [HideInInspector] private float jumpRayOffset = 0f;

    [Header("In-Game Values")]
    [SerializeField] public int jumpCount = 0;


    [Header("Movement Input Status")]
    public bool moveLeft = false;
    public bool moveRight = false;
    public bool jump = false;

    [Header("Environment Status")]
    public bool flippedSides = false;

    //reference to the character this script operates on.
    CharacterController controller;

    public MovementManager(CharacterController controller, CharacterDataSO data)
    {
        this.controller = controller;

        acceleration = data.acceleration;
        maxSpeed = data.maxSpeed;
        jumpForce = data.jumpForce;
        maxJumpCount = data.jumpCount;
    }

    public void AddGravityMultiplier()
    {
        controller.rb.AddForce(new Vector3(0, -gravityMultiplier, 0), ForceMode.Force);
    }

    public void JumpCooldown()
    {
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;
        if (jumpCooldownTimer < 0)
            jumpCooldownTimer = 0;
    }

    public void UpdatePlayerMovement()
    {

        if (moveLeft && !(((controller.isPlayer1 && !flippedSides) || (!controller.isPlayer1 && flippedSides)) && CameraController.playerDistance >= CameraController.maxPlayerDistance))
            controller.rb.AddForce(Vector3.left * acceleration, ForceMode.Acceleration);

        if (moveRight && !(((controller.isPlayer1 && flippedSides) || (!controller.isPlayer1 && !flippedSides)) && CameraController.playerDistance >= CameraController.maxPlayerDistance))
            controller.rb.AddForce(Vector3.right * acceleration, ForceMode.Acceleration);

        //cap movement speed
        Vector3 horizontalVelocity = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z);
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
        controller.rb.velocity = new Vector3(horizontalVelocity.x, controller.rb.velocity.y, horizontalVelocity.z);
        Debug.Log("velocity: " + controller.rb.velocity.normalized + " speed: " + controller.rb.velocity.magnitude);


    }

    public void Jump(KeyCode jumpKey)
    {
        //Ground Detection
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(controller.rb.transform.position.x, controller.rb.transform.position.y - jumpRayOffset, controller.rb.transform.position.z), -Vector3.up, out hit, 0.1f))
        {
            if (hit.collider.CompareTag("Ground") && jumpCooldownTimer <= 0)
            {
                jumpCount = maxJumpCount;
            }
        }
        Debug.DrawLine(new Vector3(controller.rb.transform.position.x, controller.rb.transform.position.y - jumpRayOffset, controller.rb.transform.position.z), new Vector3(controller.rb.transform.position.x, controller.rb.transform.position.y - jumpRayOffset - 0.1f, controller.rb.transform.position.z), Color.cyan);

        //Jump Logic
        if (jumpCount > 0)
        {
            if (jumpCount < maxJumpCount && Input.GetKeyDown(jumpKey))
            {
                jumpCount--;
                controller.rb.velocity = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z);
                controller.rb.AddForce(new Vector3(0, jumpForce * 0.65f, 0), ForceMode.Impulse);
                Debug.Log("Little Jump!");
                jump = false;
            }
            else if (jump && jumpCount == maxJumpCount && jumpCooldownTimer <= 0)
            {
                jumpCount--;
                jumpCooldownTimer = 0.05f;
                controller.rb.velocity = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z);
                controller.rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                Debug.Log("Big Jump!");
                jump = false;
            }
        }
    }
}