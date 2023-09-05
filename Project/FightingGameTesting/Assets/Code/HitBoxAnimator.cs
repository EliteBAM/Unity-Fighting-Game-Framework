using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAnimator : MonoBehaviour
{

    public List<GameHitBox> hitBoxRepository;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHitBox(HitBoxData data, ulong frameId, int index)
    {
        //frame = frameId % clipFrameLength

        for(int i = 0; i < data.sizeChangeFrames.Length; i++)
        {
            if (data.sizeChangeFrames[i] == (int)frameId)
            {
                hitBoxRepository[index].center = data.center[i];
                hitBoxRepository[index].size = data.size[i];
                Debug.Log("Size has been updated on frame: " + frameId);
            }
        }

    }

    public void AddHitBoxes(int hitBoxCount)
    {

        for (int i = hitBoxRepository.Count; i < hitBoxCount; i++)
        {
            GameObject newEmpty = new GameObject("Empty HitBox :D");
            newEmpty.transform.SetParent(transform);
            newEmpty.transform.localPosition = Vector3.zero;
            //just for fun:
            GameHitBox script = newEmpty.AddComponent<GameHitBox>();

            Debug.Log("adding game hitbox to repository");
            hitBoxRepository.Add(script);
            Debug.Log(hitBoxRepository.Count);
        }
    }
}
