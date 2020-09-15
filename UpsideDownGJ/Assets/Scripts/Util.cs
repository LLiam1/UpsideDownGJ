using System;
using UnityEngine;

public static class Util
{
    public static float CalculateDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(Vector2.Distance(a, b));
    }
}
