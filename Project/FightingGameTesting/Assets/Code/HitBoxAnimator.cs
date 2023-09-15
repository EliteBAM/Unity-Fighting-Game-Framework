using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAnimator : MonoBehaviour
{

    public List<HitBox> hitBoxRepository;

    public void ChangeBoxType(HitBoxData data, int index)
    {
        hitBoxRepository[index].type = data.boxType;
    }

    public void UpdateHitBox(HitBoxData data, int currentFrame, int index)
    {
        //frame = frameId % clipFrameLength
        hitBoxRepository[index].collider.enabled = (currentFrame >= data.startFrame) && (currentFrame <= data.endFrame);

        for (int i = 0; i < data.sizeChangeFrames.Length; i++)
        {
            if (data.sizeChangeFrames[i] == currentFrame)
            {
                hitBoxRepository[index].center = data.center[i];
                hitBoxRepository[index].size = data.size[i];

                Debug.Log("Size has been updated on frame: " + currentFrame);
            }
        }

    }

    public void AddHitBoxes(int hitBoxCount)
    {

        for (int i = hitBoxRepository.Count; i < hitBoxCount; i++)
        {
            GameObject newEmpty = new GameObject(); //GameObject.CreatePrimitive(PrimitiveType.Cube);
            newEmpty.name = "HitBox " + (i + 1);
            newEmpty.AddComponent<BoxCollider>().enabled = false;
            newEmpty.transform.SetParent(transform);
            newEmpty.transform.localPosition = Vector3.zero;
            //just for fun:
            HitBox script = newEmpty.AddComponent<HitBox>();

            Debug.Log("adding game hitbox to repository");
            hitBoxRepository.Add(script);
            Debug.Log(hitBoxRepository.Count);
        }
    }
}
