using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitBoxData
{
    public HitBoxType boxType;

    public int startFrame;

    public int endFrame;

    public int[] sizeChangeFrames;

    public Vector2[] center;
    public Vector2[] size;

}

[System.Serializable]
public enum HitBoxType
{
    Hit,
    Hurt
}
