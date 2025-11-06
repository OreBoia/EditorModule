using System;
using UnityEngine;

/// <summary>
/// Attributo per creare un range slider bidirezionale per MinMaxRange
/// </summary>
public class MinMaxRangeAttribute : PropertyAttribute
{
    public float min;
    public float max;

    public MinMaxRangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}