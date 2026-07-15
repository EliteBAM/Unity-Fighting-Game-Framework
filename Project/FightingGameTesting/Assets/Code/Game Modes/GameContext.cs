using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{

    public List<GameObject> sceneObjects;

    public List<bool> conditions;


    ComponentType GetContextComponent<ComponentType>(string tag)
    {

        foreach (GameObject obj in sceneObjects)
        {
            if (!obj.tag.Equals(tag))
                break;

            ComponentType context = obj.GetComponent<ComponentType>();
            if (context != null)
                return context;
        }

        return default;
    }

    //GetContextCondition

}
