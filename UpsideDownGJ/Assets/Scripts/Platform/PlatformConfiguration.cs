using System;
using UnityEngine;

[Serializable]
public class PlatformConfiguration 
{
    public float rangeX;
    public float rangeY;
    public float moveSpeed;

    public Vector3 centerPos;

    public PlatformState direction;
}
