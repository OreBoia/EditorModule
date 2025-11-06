using System;
using UnityEngine;

[System.Serializable]
public struct Vector2i
{
    public int x;
    public int y;

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2i zero => new Vector2i(0, 0);
    public static Vector2i one => new Vector2i(1, 1);

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}