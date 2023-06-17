using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

[Serializable]
public class InputManager
{
    //History of past 5 inputs
    Queue<SequenceBlock> inputHistory = new Queue<SequenceBlock>();
    int maxHistorySize = 5;

    //Data of which controls to check for
    public ControlSet playerControls;

    //Current input Data
    public float inputTimer = 0;
    SequenceBlock currentInput;

    //root node and traversal node
    Node activeNode;
    Node root;

    public float currentFrameWindow;
    Dictionary<GenericControls, KeyCode> keybindReferences;

    private AnimationManager animationManager;

    public InputManager(ControlSet set, ComboTree tree, AnimationManager animationManager)
    {
        playerControls = set;
        root = tree.root;
        activeNode = root;
        this.animationManager = animationManager;

        keybindReferences = GetKeybindReference();
    }

    // Update is called once per frame
    public void UpdateInputs()
    {
        UpdateCurrentFrameWindow();

        if (TryGetPlayerInput(out currentInput))
        {
            NextNode(currentInput);
            UpdateInputHistory(currentInput);
        }
    }

    void UpdateCurrentFrameWindow ()
    {
        if (!activeNode.isRoot)
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > currentFrameWindow)
                ResetTree();
        }
    }

    Dictionary<GenericControls, KeyCode> GetKeybindReference ()
    {
        Dictionary<GenericControls, KeyCode> references = new Dictionary<GenericControls, KeyCode>();

        foreach (var field in typeof(ControlSet).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            string control = field.Name;
            KeyCode keybind = (KeyCode)field.GetValue(playerControls);

            GenericControls output;
            if (Enum.TryParse(control, out output))
                references.Add(output, keybind);
        }

        return references;
    }

    bool TryGetPlayerInput(out SequenceBlock inputBlock)
    {
        List<GenericControls> pressedKeys = new List<GenericControls>();

        foreach (var reference in keybindReferences)
        {

            if (Input.GetKeyDown(reference.Value)
                    || (Input.GetKey(reference.Value) && pressedKeys.Count > 0 && (reference.Key == GenericControls.Left || reference.Key == GenericControls.Right || reference.Key == GenericControls.Duck)))
            {
                pressedKeys.Add(reference.Key);
            }

            /*if (Input.GetKeyUp(reference.Value) && (reference.Key == GenericControls.Left || reference.Key == GenericControls.Right)
                    && pressedKeys.Count == 0)
            {
                animationManager.PlayAnimation("_idle");
            }*/
        }

        if (pressedKeys.Count == 0)
        {
            inputBlock = null;
            return false;
        }
        else
        {
            inputBlock = new SequenceBlock(pressedKeys.ToArray(), inputTimer);
            Debug.Log("player input recieved");
            return true;
        }
    }

    public void ResetTree()
    {
        activeNode = root;
        currentFrameWindow = 0;
        inputTimer = 0;
        Debug.Log("tree reset.");
    }

    public void NextNode(SequenceBlock input)
    {
        for (int i = 0; i < activeNode.children.Count; i++)
        {
            if (input.CompareKeys(activeNode.children[i].sequenceBlockData) && inputTimer <= currentFrameWindow)
            {
                activeNode = activeNode.children[i];
                ExecuteMove();

                inputTimer = 0;
                currentFrameWindow = activeNode.sequenceBlockData.frameWindow;
                break;
            }
        }
    }

    public void ExecuteMove()
    {
        if (activeNode.containsMove)
        {
            animationManager.PlayAnimation(activeNode.moveAnim.name);
            //anim.Play(activeNode.moveAnim.name);
            Debug.Log(activeNode.moveName + " was Activated!");
        }
    }

    void UpdateInputHistory(SequenceBlock input)
    {
        inputHistory.Enqueue(input);

        if (inputHistory.Count > maxHistorySize)
            inputHistory.Dequeue();

        //debug
        if (inputHistory.Count > 0)
        {
            string debug = "[ ";
            foreach (SequenceBlock sequence in inputHistory)
            {
                debug = debug + sequence.SequenceBlockDebug() + " ";
            }
            debug = debug + "]";
            Debug.Log(debug);
        }
    }

}
