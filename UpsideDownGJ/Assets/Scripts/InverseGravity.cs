using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GrabbableItem))]
public class InverseGravity : MonoBehaviour
{
    private GrabbableItem gravityObject;
    private GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        gravityObject = GetComponent<GrabbableItem>();
        gravityObject.SetGravityScale(gravityObject.GetGravityScale() * -1);

        controller = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SameDirection(gravityObject.GetGravityScale(), controller.GetGravityDirection())) {
            gravityObject.SetGravityScale(gravityObject.GetGravityScale() * -1);
        }
    }

    private bool SameDirection(float gravityScale, float otherGravityScale)
    {
        return (gravityScale > 0 && otherGravityScale > 0)
            || (gravityScale < 0 && otherGravityScale < 0);
    }
}
