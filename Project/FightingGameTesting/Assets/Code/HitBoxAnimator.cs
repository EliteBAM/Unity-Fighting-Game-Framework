using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAnimator : MonoBehaviour
{

    public List<GameObject> hitBoxRepository;

    void Awake()
    {
        hitBoxRepository = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHitBox()
    {
        
    }

    public void AddHitBoxes(int hitBoxCount)
    {
        for(int i = hitBoxRepository.Count; i < hitBoxCount; i++)
        {
            GameObject newEmpty = new GameObject("Empty HitBox");
            newEmpty.transform.SetParent(transform);
            newEmpty.transform.localPosition = Vector3.zero;
            //just for fun:
            //newEmpty.AddComponent<HitBoxTesting>();

            hitBoxRepository.Add(newEmpty);
        }
    }
}
