using System;
using UnityEngine;

[System.Serializable]
public struct MinMaxRange
{
    public float min;
    public float max;

    public MinMaxRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float RandomValue => UnityEngine.Random.Range(min, max);

    public bool Contains(float value)
    {
        return value >= min && value <= max;
    }

    public override string ToString()
    {
        return $"Min: {min}, Max: {max}";
    }
}