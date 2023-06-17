using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool isPlayer1;

    [Header("Specialized Character Data:")]
    [SerializeField] public CharacterDataSO characterData;

    [Header("Controller Modules:")]
    [SerializeField] public InputManager inputManager;

    [SerializeField] public MovementManager movementManager;

    [SerializeField] public AnimationManager animationManager;

    //GameObject Components
    [HideInInspector] public Rigidbody rb;
    Animator anim;

    //special data structures
    public ComboTree comboTree;
    private ControlSet keybinds;

    //misc variables
    private string moveLeftAnim;
    private string moveRightAnim;


    // Start is called before the first frame update
    void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        InitializeCharacterSettings();

        InitializeCharacterData();

    }

    // Update is called once per frame
    void Update()
    {
        ResetPosition();

        FlipCharacter();

        UpdateBasicInputs();

        inputManager.UpdateInputs();

        movementManager.JumpCooldown();

        movementManager.Jump(keybinds.Jump);

        animationManager.UpdateAnimationGraph();

    }

    void FixedUpdate()
    {
        movementManager.AddGravityMultiplier();
        movementManager.UpdatePlayerMovement();
    }

    public void ResetPosition()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlayer1)
                transform.position = new Vector3(-5, 1.1f, 0);
            else
                transform.position = new Vector3(5, 1.1f, 0);
        }
    }

    public void InitializeCharacterSettings()
    {
        if (CompareTag("Player 1"))
        {
            isPlayer1 = true;
            keybinds = ControlSettings.Keyboard_Player_1;
            moveLeftAnim = "_back";
            moveRightAnim = "_forward";
        }
        else
        {
            isPlayer1 = false;
            keybinds = ControlSettings.Keyboard_Player_2;
            moveLeftAnim = "_forward";
            moveRightAnim = "_back";
        }
    }

    public void InitializeCharacterData()
    {
        if (characterData != null)
        {

            comboTree = new ComboTree(characterData.moveList);

            animationManager = new AnimationManager(anim, characterData);

            movementManager = new MovementManager(this, characterData);

            inputManager = new InputManager(keybinds, comboTree, animationManager);


            Debug.Log(characterData.characterName + " Data Initialized.");

        }
        else
        {
            Debug.LogWarning("No Character Data Has Been Loaded.");
        }
    }

    private void UpdateBasicInputs()
    {
        //left
        if (Input.GetKeyDown(keybinds.Right))
        {
            movementManager.moveRight = true;
            anim.applyRootMotion = false;
            animationManager.PlayAnimation(moveRightAnim);
        }

        if (Input.GetKey(keybinds.Right))
        {
            if(Input.GetKeyDown(keybinds.Left))
            {
                movementManager.moveRight = false;

                anim.applyRootMotion = true;
                animationManager.PlayAnimation("_idle");
                return;
            }
        }

        //right
        if (Input.GetKeyDown(keybinds.Left))
        {
            movementManager.moveLeft = true;
            anim.applyRootMotion = false;
            animationManager.PlayAnimation(moveLeftAnim);
        }

        if (Input.GetKey(keybinds.Left))
        {
            if (Input.GetKeyDown(keybinds.Right))
            {
                movementManager.moveLeft = false;
                movementManager.moveRight = false; //don't know why I need this line here, but it works fine in the opposite direction without it.
                //I figured it out. It's because of the order of the if statements. I guess it's fine though even though the asymetry bothers me?

                anim.applyRootMotion = true;
                animationManager.PlayAnimation("_idle");
                return;
            }
        }


        //jump
        if (Input.GetKey(keybinds.Jump))
            movementManager.jump = true;

        //unpress
        if (Input.GetKeyUp(keybinds.Left))
        {
            movementManager.moveLeft = false;
            if(!Input.GetKey(keybinds.Right))
            {
                anim.applyRootMotion = true;
                animationManager.PlayAnimation("_idle");
            }
            else
            {
                movementManager.moveRight = true;
                anim.applyRootMotion = false;
                animationManager.PlayAnimation(moveRightAnim);
            }
        }

        if (Input.GetKeyUp(keybinds.Right))
        {
            movementManager.moveRight = false;
            if (!Input.GetKey(keybinds.Left))
            {
                anim.applyRootMotion = true;
                animationManager.PlayAnimation("_idle");
            }
            else
            {
                movementManager.moveLeft = true;
                anim.applyRootMotion = false;
                animationManager.PlayAnimation(moveLeftAnim);
            }
        }

        if (Input.GetKeyUp(keybinds.Jump))
            movementManager.jump = false;
    }

    private void FlipCharacter()
    {
        if(CameraController.flipP1 || CameraController.flipP2)
        {
            var tempString = moveRightAnim;
            moveRightAnim = moveLeftAnim;
            moveLeftAnim = tempString;

            transform.Rotate(0, 180, 0);

            if (isPlayer1)
                CameraController.flipP1 = false;
            else
                CameraController.flipP2 = false;

            if (movementManager.moveRight)
                animationManager.PlayAnimation(moveRightAnim);
            else if (movementManager.moveLeft)
                animationManager.PlayAnimation(moveLeftAnim);

            movementManager.flippedSides = !movementManager.flippedSides;
        }
    }



    /////////////////////////////////////////////////////////
    ///// V //////Animation Graph Functionality////// V /////
    /////////////////////////////////////////////////////////
    public void DisplayAnimationGraph()
    {
        animationManager.DisplayAnimationGraph();
    }

    private void OnDestroy()
    {
        //Memory clean-up for low-level Playables API
        animationManager.DestroyAnimationGraph();
    }
}
