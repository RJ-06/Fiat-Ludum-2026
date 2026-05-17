using UnityEngine;

[System.Serializable]
public class SliceZone
{
    public float position;
    public float radius;

    public SliceZone(float position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }

    public bool IsInside(float value)
    {
        return Mathf.Abs(value - position) <= radius;
    }
}