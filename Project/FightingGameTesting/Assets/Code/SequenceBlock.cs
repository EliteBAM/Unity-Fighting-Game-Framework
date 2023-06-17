using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class SequenceBlock
{
    public GenericControls[] keys;
    public float frameWindow;

    public SequenceBlock(GenericControls[] keys, float frameWindow)
    {
        this.keys = keys;
        this.frameWindow = frameWindow;
    }

    public SequenceBlock()
    {
        keys = new GenericControls[0];
        frameWindow = 0;
    }

    public bool CompareKeys(SequenceBlock block)
    {
        if (keys.Length != block.keys.Length)
        {
            return false;
        }
        else
        {
            // Create a Dictionary to count the number of occurrences of each element in the other object's keys
            Dictionary<GenericControls, int> keysCounts = new Dictionary<GenericControls, int>();

            foreach (var keyCount in block.keys)
            {
                if (!keysCounts.ContainsKey(keyCount))
                    keysCounts[keyCount] = 1;
                else
                    keysCounts[keyCount]++;
            }

            // Loop through this object's keys
            for(int i = 0; i < keys.Length; i++)
            {
                // If the Dictionary does not contain the element, or if its count is 0, return false
                if (!keysCounts.ContainsKey(keys[i]) || keysCounts[keys[i]] == 0)
                    return false;
                else
                    keysCounts[keys[i]]--;      // If the Dictionary contains the element and its count is greater than 0, decrement its count
            }

            // If all elements in this object's keys have been found in the other block's keys, return true
            return true;
        }
    }

    public string SequenceBlockDebug()
    {
        string keysString = "";
        for (int i = 0; i < keys.Length; i++)
        {
            if(i == keys.Length-1)
                keysString = keysString + keys[i].ToString();
            else
                keysString = keysString + keys[i].ToString() + ", ";
        }

         return "( " + keysString + " )";
    }

}
