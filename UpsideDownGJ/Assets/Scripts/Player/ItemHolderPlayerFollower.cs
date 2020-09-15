using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderPlayerFollower : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();   
    }

    // Update is called once per frame
    void Update()
    {
        var xPos = GetTrueXPosition();
        var yPos = GetTrueYPosition();

        //Debug.Log(xPos);
        if (player.FlippedX())
        {
            //Debug.Log("flipped x");
            xPos *= -1;
        }
        //Debug.Log(xPos);

        if (player.FlippedY())
        {
            yPos *= -1;
        }

        transform.localPosition = new Vector2(xPos, yPos);
    }

    private float GetTrueXPosition()
    {
        return Mathf.Abs(transform.localPosition.x);
    }

    private float GetTrueYPosition()
    {
        return Mathf.Abs(transform.localPosition.y);
    }
}
