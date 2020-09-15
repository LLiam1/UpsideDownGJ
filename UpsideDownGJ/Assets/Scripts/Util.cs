using System;
using UnityEngine;

public static class Util
{
    public static float CalculateDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(Vector2.Distance(a, b));
    }

    public static Vector2 GetDirectionVector2D(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
}
