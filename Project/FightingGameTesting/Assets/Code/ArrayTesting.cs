using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ArrayTesting : MonoBehaviour
{
    int[] array1 = { 1, 2, 3, 4, 5 };
    int[] array2 = { 5, 4, 3, 2, 1 };

    GenericControls[] keys1 = { GenericControls.West, GenericControls.North, GenericControls.South, GenericControls.East };
    GenericControls[] keys2 = { GenericControls.East, GenericControls.South, GenericControls.North, GenericControls.West };

    GenericControls[] keys3 = { GenericControls.West, GenericControls.North, GenericControls.South, GenericControls.East };
    GenericControls[] keys4 = { GenericControls.West, GenericControls.North, GenericControls.South, GenericControls.East };

    GenericControls[] keys5 = { GenericControls.West, GenericControls.South, GenericControls.South, GenericControls.East };
    GenericControls[] keys6 = { GenericControls.West, GenericControls.North, GenericControls.South, GenericControls.East };

    GenericControls[] keys7 = { GenericControls.West, GenericControls.West, GenericControls.South, GenericControls.East };
    GenericControls[] keys8 = { GenericControls.West, GenericControls.South, GenericControls.South, GenericControls.East };



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initial Arrays:");
        Debug.Log(PrintArray(keys1));
        Debug.Log(PrintArray(keys2));

        Debug.Log("Compare Arrays:");
        Debug.Log(keys1.SequenceEqual(keys2));

        Debug.Log("Compare Arrays Order Excluded:");
        Debug.Log(CompareKeysRevised(keys1, keys2));
        Debug.Log(CompareKeys(keys1, keys2));
        //Debug.Log(AltCompareKeys(keys1, keys2));
        Debug.Log(AltAltCompareKeys(keys1, keys2));


        //////////
        Debug.Log("Initial Arrays:");
        Debug.Log(PrintArray(keys3));
        Debug.Log(PrintArray(keys4));

        Debug.Log("Compare Arrays:");
        Debug.Log(keys3.SequenceEqual(keys4));

        Debug.Log("Compare Arrays Order Excluded:");
        Debug.Log(CompareKeysRevised(keys3, keys4));
        Debug.Log(CompareKeys(keys3, keys4));
        //Debug.Log(AltCompareKeys(keys3, keys4));
        Debug.Log(AltAltCompareKeys(keys3, keys4));


        //////////
        Debug.Log("Initial Arrays:");
        Debug.Log(PrintArray(keys5));
        Debug.Log(PrintArray(keys6));

        Debug.Log("Compare Arrays:");
        Debug.Log(keys5.SequenceEqual(keys6));

        Debug.Log("Compare Arrays Order Excluded:");
        Debug.Log(CompareKeysRevised(keys5, keys6));
        Debug.Log(CompareKeys(keys5, keys6));
        //Debug.Log(AltCompareKeys(keys5, keys6));
        Debug.Log(AltAltCompareKeys(keys5, keys6));


        //////////
        Debug.Log("Initial Arrays:");
        Debug.Log(PrintArray(keys7));
        Debug.Log(PrintArray(keys8));

        Debug.Log("Compare Arrays:");
        Debug.Log(keys7.SequenceEqual(keys8));

        Debug.Log("Compare Arrays Order Excluded:");
        Debug.Log(CompareKeysRevised(keys7, keys8));
        Debug.Log(CompareKeys(keys7, keys8));
        //Debug.Log(AltCompareKeys(keys7, keys8));
        Debug.Log(AltAltCompareKeys(keys7, keys8));



    }

    string PrintArray(GenericControls[] arr)
    {
        string printedArr = "[ ";
        for (int i = 0; i < arr.Length; i++)
        {
            if(i < arr.Length-1)
                printedArr += arr[i].ToString() + ", ";
            else
                printedArr += arr[i].ToString() + " ]";
        }

        return printedArr;
    }

    public bool CompareKeysRevised(GenericControls[] keys1, GenericControls[] arr2)
    {

        if (keys1.Length != keys2.Length)
        {
            return false;
        }
        else
        {
            GenericControls[] keys2 = (GenericControls[])arr2.Clone();

            for (int i = 0; i < keys1.Length; i++)
            {
                if (!keys2.Contains(keys1[i]))
                {
                    return false; //because if it makes it to the last element in keys2 and still hasnt found a match then a missing element is confirmed
                }
                else
                {
                    keys2[Array.IndexOf(keys2, keys1[i])] = GenericControls.None;
                    continue;
                }
            }

            return true;
        }
    }

    public bool CompareKeys(GenericControls[] keys1, GenericControls[] arr2)
    {

        if (keys1.Length != keys2.Length)
        {
            return false;
        }
        else
        {
            GenericControls[] keys2 = (GenericControls[])arr2.Clone();

            for (int i = 0; i < keys1.Length; i++)
            {
                for (int j = 0; j < keys1.Length; j++)
                {
                    if (keys1[i] != keys2[j])
                    {
                        if (j == keys1.Length - 1)
                            return false; //because if it makes it to the last element in keys2 and still hasnt found a match then a missing element is confirmed

                        continue;
                    }
                    else
                    {
                        keys2[j] = GenericControls.None;
                        break;
                    }
                }
            }

            return true;
        }
    }

    public bool AltCompareKeys(GenericControls[] arr1, GenericControls[] arr2)
    {

        if (arr1.Length != arr2.Length)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr2.Contains(arr1[i]) && arr1.Contains(arr2[i]))
                    continue;
                else
                    return false;
            }

            return true;
        }
    }   

    public bool AltAltCompareKeys(GenericControls[] array1, GenericControls[] array2)
    {
        // Create a Dictionary to count the number of occurrences of each element in array2

        if (array1.Length != array2.Length)
        {
            return false;
        }
        else
        {
            Dictionary<GenericControls, int> keyCounts = new Dictionary<GenericControls, int>();

            foreach (var element in array2)
            {
                if (!keyCounts.ContainsKey(element))
                {
                    // If the Dictionary does not contain the element, add it with a count of 1
                    keyCounts[element] = 1;
                }
                else
                {
                    // If the Dictionary already contains the element, increment its count
                    keyCounts[element]++;
                }
            }

            // Loop through array1
            foreach (var element in array1)
            {
                // If the Dictionary does not contain the element, or if its count is 0, return false
                if (!keyCounts.ContainsKey(element) || keyCounts[element] == 0)
                {
                    return false;
                }
                else
                {
                    // If the Dictionary contains the element and its count is greater than 0, decrement its count
                    keyCounts[element]--;
                }
            }

            // If all elements in array1 have been found in array2, return true
            return true;
        }
    }



    void ModifyArrayInMethod(int[] paramArray)
    {
        Debug.Log("Setting array element to 0 in method");
        paramArray[3] = 0;

        Debug.Log("original Array 1:");
        foreach (var item in array1)
        {
            Debug.Log(item);
        }
        Debug.Log("Array in Method: ");
        foreach (var item in paramArray)
        {
            Debug.Log(item);
        }
    }

    void ChangeArrayReference(int[] paramArray)
    {
        Debug.Log("Changing array 1 to be 0, 9, 8");
        paramArray = new int[] { 0, 9, 8 };
        
        Debug.Log("original Array 1:");
        foreach (var item in array1)
        {
            Debug.Log(item);
        }
        Debug.Log("Array in Method: ");
        foreach (var item in paramArray)
        {
            Debug.Log(item);
        }
    }

    void RefChangeArrayReference(ref int[] paramArray)
    {
        Debug.Log("Changing array 1 to be 0, 9, 8");
        paramArray = new int[] { 0, 9, 8 };

        Debug.Log("original Array 1:");
        foreach (var item in array1)
        {
            Debug.Log(item);
        }
        Debug.Log("Array in Method: ");
        foreach (var item in paramArray)
        {
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
