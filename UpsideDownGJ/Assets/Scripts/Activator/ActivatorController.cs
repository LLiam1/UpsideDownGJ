﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorController : MonoBehaviour
{
    public ActivatorConfiguration config;
    public ButtonActivatedGate gate;

    private void Start()
    {
        if (config.door != null)
        {
            config.doorController = config.door.GetComponent<DoorController>();
        }
    }

    private void Update()
    {
        if (config.doorController != null)
        {
            config.doorController.isOpened = config.isActivated;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        config.button.transform.position -= new Vector3(0, 0.15f, 0);
        config.isActivated = true;
        if (gate != null)
        {
            gate.Activate();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        config.isActivated = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        config.button.transform.position += new Vector3(0, 0.15f, 0);
        config.isActivated = false;
        if (gate != null)
        {
            gate.Activate();
        }
    }
}
