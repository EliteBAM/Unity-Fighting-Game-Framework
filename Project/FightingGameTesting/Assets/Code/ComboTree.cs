using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTree
{
    public Node root;

    public ComboTree (Move[] moveList)
    {
        root = new Node(new SequenceBlock());
        root.isRoot = true;

        GenerateTree(moveList);
        Debug.Log("Combo Tree has " + GetNodeCount(root) + " nodes.");
    }

    public void GenerateTree(Move[] moveList)
    {
        Node current;
        Node previous;

        for(int i = 0; i < moveList.Length; i++)
        {
            Move currentMove = moveList[i];
            List<SequenceBlock> currentSequence = currentMove.inputSequence;

            previous = root;

            for (int j = 0; j < currentSequence.Count; j++)
            {
                current = new Node(currentSequence[j]);
                current.associatedMove = currentMove.moveName;

                float currentFrameWindow = current.sequenceBlockData.frameWindow;

                //check for duplicate node already existing in the same depth region of the tree
                bool alreadyInserted = false;

                if (previous.children.Count != 0)
                {
                    for(int k = 0; k < previous.children.Count; k++)
                    {
                        float childFramWindow = previous.children[k].sequenceBlockData.frameWindow;

                        //comparing the sequence block data of the children nodes and current node
                        //the current node will not be added if a child is found to be identical
                        if (current.sequenceBlockData.CompareKeys(previous.children[k].sequenceBlockData))
                        {
                            alreadyInserted = true;

                            //set previous node for next iteration to be the already existing identical node...
                            //...instead of the current node which will be discarded.
                            previous = previous.children[k];
                        }
                    }
                }

                //Add node to tree if identical node has not already been inserted
                if (alreadyInserted == false)
                {
                    current.parent = previous;
                    previous.children.Add(current);
                    previous = current;

                    //Store move data in final node of a move
                    //this is so you can access the move data if the move is landed on in the tree
                    if (j == (currentSequence.Count - 1))
                    {
                        current.containsMove = true;
                        current.moveAnim = currentMove.moveAnim.clip;
                        current.moveName = currentMove.moveName;
                        current.hitAnim = currentMove.hitAnim.clip;
                    }
                }
            }
        }
    }

    public int GetNodeCount(Node current)
    {
        int nodeCount = 0;
        if(current == null)
            return nodeCount;
        else
        {
            //Add one to the count for root node
            nodeCount++;

            //recurse through all child nodes
            if(current.children.Count != 0)
            {
                for (int i = 0; i < current.children.Count; i++)
                {
                    nodeCount += GetNodeCount(current.children[i]);
                    Debug.Log(current.children[i].moveName + ": " + current.children[i].sequenceBlockData.frameWindow);
                }
            }
        }

        return nodeCount;
    }

}

public class Node
{
    public bool isRoot = false;
    public bool containsMove = false;

    public Node parent = null;
    public List<Node> children = new List<Node>();

    //Move Data (initialized if final block of combo, else null)
    public string moveName = null;
    public AnimationClip moveAnim = null;
    public AnimationClip hitAnim = null;

    public SequenceBlock sequenceBlockData; // { get; set; }
    public string associatedMove;



    public Node (SequenceBlock sequence)
    {
        sequenceBlockData = sequence;
    }
}
