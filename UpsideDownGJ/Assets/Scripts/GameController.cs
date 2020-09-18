﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool switched;

    private Rigidbody2D[] rigidBodies;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = FindObjectsOfType<Rigidbody2D>();
        switched = false;
    }

    public void SwitchGravity()
    {
        switched = !switched;
        foreach(var rb in rigidBodies)
        {
            if (!rb.simulated)
            {
                continue;
            }

            var gravityObject = rb.gameObject.GetComponent<IGravityObject>();
            if (gravityObject != null && !gravityObject.UsingGravity())
            {
                continue;
            }

            rb.gravityScale *= -1;
        }
    }

    public int GetGravityDirection()
    {
        return switched ? -1 : 1;
    }
}
