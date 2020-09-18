using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DoorConfiguration
{
    public float rangeX;
    public float rangeY;
    public float moveSpeed;

    public Vector3 centerPos;
    public DoorState direction;
}
