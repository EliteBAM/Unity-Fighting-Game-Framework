using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(HitBoxAnimator))]
public class CharacterController : MonoBehaviour
{
    public bool isPlayer1;

    [Header("Game Stats")]
    public int health;
    public int score;
    public int streak;

    [HideInInspector] public int maxHealth = 100;

    [Header("Specialized Character Data:")]
    [SerializeField] public CharacterDataSO characterData;
    public bool useCharacterRig = true;
    private CharacterDataSO enemyData; //used only to get hit reaction animations from the enemy's move list

    [Header("High Level Managers")]
    [SerializeField] public PlayerStateManager stateManager;

    [Header("Controller Modules:")]
    [SerializeField] public InputManager inputManager;

    [SerializeField] public MovementManager movementManager;

    [SerializeField] public AnimationManager animationManager;

    //GameObject Components
    [HideInInspector] public Rigidbody rb;
    public Animator anim;
    HitBoxAnimator hitBoxAnim;

    //special data structures
    public ComboTree comboTree;
    private ControlSet keybinds;

    //misc variables
    private string moveLeftAnim;
    private string moveRightAnim;


    // Start is called before the first frame update
    void Awake()
    {

        anim = GetComponent<Animator>();
        hitBoxAnim = GetComponentInChildren<HitBoxAnimator>();
        rb = GetComponent<Rigidbody>();

        InitializeCharacterSettings();

        GetEnemyData();

        InitializeCharacterData();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ResetPosition();

        FlipCharacter();

        UpdateBasicInputs();

        inputManager.UpdateInputs();

        movementManager.JumpCooldown();

        movementManager.Jump(keybinds.Jump);

        animationManager.UpdateAnimationGraph();

        stateManager.UpdateCurrentState();

    }

    void FixedUpdate()
    {
        movementManager.AddGravityMultiplier();
        movementManager.UpdatePlayerMovement();
    }

    public void ResetPosition()
    {
        if (isPlayer1)
            transform.position = new Vector3(-5, 1.1f, 0);
        else
            transform.position = new Vector3(5, 1.1f, 0);
    }

    public void GetEnemyData()
    {
        string tag;

        if (isPlayer1)
            tag = "Player 2";
        else
            tag = "Player 1";

        if (GameObject.FindGameObjectWithTag(tag) != null)
            enemyData = GameObject.FindGameObjectWithTag(tag).GetComponent<CharacterController>().characterData;
        else
            enemyData = null;
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
            //setting up the character model first
            if(useCharacterRig)
            {
                GameObject rig = Instantiate(characterData.characterRig, transform.position, transform.rotation);
                Destroy(rig.GetComponent<Animator>());
                rig.transform.parent = transform; //setting model as child of player object
                rig.transform.localScale = Vector3.one * characterData.modelScale; //setting scale of model based on data sheet

                //getting the children and changing the parent
                int childrenCount = rig.transform.childCount;
                Transform[] children = new Transform[childrenCount];
                for (int i = 0; i < childrenCount; i++)
                    children[i] = rig.transform.GetChild(i);
                foreach (Transform child in children)
                    child.SetParent(transform);

                Destroy(rig); //destroying the remaining empty GameObject
                anim.avatar = characterData.rigAvatar; //setting the avatar based on data sheet.
            }


            //setting up stats
            maxHealth = characterData.maxHealth;
            health = maxHealth;

            comboTree = new ComboTree(characterData.moveList);

            animationManager = new AnimationManager(anim, hitBoxAnim, characterData, enemyData);

            movementManager = new MovementManager(this, characterData);

            stateManager = new PlayerStateManager(animationManager);

            inputManager = new InputManager(keybinds, comboTree, stateManager);


            Debug.Log(characterData.characterName + " Data Initialized.");

        }
        else
        {
            Debug.LogWarning("No Character Data Has Been Loaded.");
        }
    }

    private void UpdateBasicInputs()
    {
        //this functionality should be better implemented and better organized in the future
        //ex. this should just update inputs. probably not also update movement based on current state
        //each fight animation should probably be responsible for its own root motion type to clean this up.
            //maybe fight animations can contain metadata about whether root motion should be applied during its playback?
            //then, there would be a routine in the animator to flip the setting on and off in the "play animation" function based on the data


        if (stateManager.currentState.stateType == StateType.Combo || stateManager.currentState.stateType == StateType.Hit)
        {
            anim.applyRootMotion = true;
            movementManager.moveLeft = false;
            movementManager.moveRight = false;
        }

        if (stateManager.currentState.stateType == StateType.Idle || stateManager.currentState.stateType == StateType.Walking)
        {
            //idle if left and right are pressed
            if (Input.GetKey(keybinds.Right) && Input.GetKey(keybinds.Left) && stateManager.currentState.stateType == StateType.Walking)
            {
                movementManager.moveRight = false;
                movementManager.moveLeft = false;

                anim.applyRootMotion = true;
                //animationManager.PlayAnimation("_idle");
                stateManager.ClearQueue();
                stateManager.AddStateToQueue("_idle", null, StateType.Idle);
                stateManager.InterruptToNextState();
                Debug.Log("this should only happen once");
                return;
            }

            //right
            if (Input.GetKey(keybinds.Right) && !Input.GetKey(keybinds.Left) && stateManager.currentState.stateType == StateType.Idle)
            {
                movementManager.moveRight = true;
                anim.applyRootMotion = false;
                //animationManager.PlayAnimation(moveRightAnim);
                stateManager.ClearQueue();
                stateManager.AddStateToQueue(moveRightAnim, null, StateType.Walking);
                stateManager.InterruptToNextState();
            }
            else if(Input.GetKeyUp(keybinds.Right) && !Input.GetKey(keybinds.Left) && stateManager.currentState.stateType == StateType.Walking)
            {
                movementManager.moveRight = false;
                movementManager.moveLeft = false;

                anim.applyRootMotion = true;

                stateManager.ClearQueue();
                stateManager.AddStateToQueue("_idle", null, StateType.Idle);
                stateManager.InterruptToNextState();
            }

            //left
            if (Input.GetKey(keybinds.Left) && !Input.GetKey(keybinds.Right) && stateManager.currentState.stateType == StateType.Idle)
            {
                movementManager.moveLeft = true;
                anim.applyRootMotion = false;
                //animationManager.PlayAnimation(moveLeftAnim);
                stateManager.ClearQueue();
                stateManager.AddStateToQueue(moveLeftAnim, null, StateType.Walking);
                stateManager.InterruptToNextState();

            }
            else if(Input.GetKeyUp(keybinds.Left) && !Input.GetKey(keybinds.Right) && stateManager.currentState.stateType == StateType.Walking)
            {
                movementManager.moveLeft = false;
                movementManager.moveRight = false;

                anim.applyRootMotion = true;

                stateManager.ClearQueue();
                stateManager.AddStateToQueue("_idle", null, StateType.Idle);
                stateManager.InterruptToNextState();
            }


            //jump
            if (Input.GetKey(keybinds.Jump))
                movementManager.jump = true;
            if (Input.GetKeyUp(keybinds.Jump))
                movementManager.jump = false;
        }
    }

    //private void UpdateBasicInputs()
    //{
    //    if (stateManager.currentState.stateType == StateType.Combo) //this functionality should be better implemented and better organized in the future
    //        anim.applyRootMotion = true;

    //    if (stateManager.currentState.stateType == StateType.Idle)
    //    {
    //        //left
    //        if (Input.GetKeyDown(keybinds.Right))
    //        {
    //            movementManager.moveRight = true;
    //            anim.applyRootMotion = false;
    //            animationManager.PlayAnimation(moveRightAnim);
    //        }

    //        if (Input.GetKey(keybinds.Right))
    //        {
    //            if (Input.GetKeyDown(keybinds.Left))
    //            {
    //                movementManager.moveRight = false;

    //                anim.applyRootMotion = true;
    //                animationManager.PlayAnimation("_idle");
    //                return;
    //            }
    //        }

    //        //right
    //        if (Input.GetKeyDown(keybinds.Left))
    //        {
    //            movementManager.moveLeft = true;
    //            anim.applyRootMotion = false;
    //            animationManager.PlayAnimation(moveLeftAnim);
    //        }

    //        if (Input.GetKey(keybinds.Left))
    //        {
    //            if (Input.GetKeyDown(keybinds.Right))
    //            {
    //                movementManager.moveLeft = false;
    //                movementManager.moveRight = false; //don't know why I need this line here, but it works fine in the opposite direction without it.
    //                                                   //I figured it out. It's because of the order of the if statements. I guess it's fine though even though the asymmetry bothers me?

    //                anim.applyRootMotion = true;
    //                animationManager.PlayAnimation("_idle");
    //                return;
    //            }
    //        }


    //        //jump
    //        if (Input.GetKey(keybinds.Jump))
    //            movementManager.jump = true;

    //        //unpress
    //        if (Input.GetKeyUp(keybinds.Left))
    //        {
    //            movementManager.moveLeft = false;
    //            if (!Input.GetKey(keybinds.Right))
    //            {
    //                anim.applyRootMotion = true;
    //                animationManager.PlayAnimation("_idle");
    //            }
    //            else
    //            {
    //                movementManager.moveRight = true;
    //                anim.applyRootMotion = false;
    //                animationManager.PlayAnimation(moveRightAnim);
    //            }
    //        }

    //        if (Input.GetKeyUp(keybinds.Right))
    //        {
    //            movementManager.moveRight = false;
    //            if (!Input.GetKey(keybinds.Left))
    //            {
    //                anim.applyRootMotion = true;
    //                animationManager.PlayAnimation("_idle");
    //            }
    //            else
    //            {
    //                movementManager.moveLeft = true;
    //                anim.applyRootMotion = false;
    //                animationManager.PlayAnimation(moveLeftAnim);
    //            }
    //        }

    //        if (Input.GetKeyUp(keybinds.Jump))
    //            movementManager.jump = false;
    //    }
    //    else
    //    {
    //        movementManager.moveLeft = false;
    //        movementManager.moveRight = false;
    //    }
    //}

    private void FlipCharacter()
    {
        if(CombatCameraController.flipP1 || CombatCameraController.flipP2)
        {
            var tempString = moveRightAnim;
            moveRightAnim = moveLeftAnim;
            moveLeftAnim = tempString;

            transform.Rotate(0, 180, 0);

            if (isPlayer1)
                CombatCameraController.flipP1 = false;
            else
                CombatCameraController.flipP2 = false;

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
