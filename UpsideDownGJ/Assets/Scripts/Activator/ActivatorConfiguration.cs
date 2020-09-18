using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ActivatorConfiguration 
{
    public bool isActivated;
    public GameObject button;
    public GameObject door;

    [NonSerialized]
    public DoorController doorController;
}
