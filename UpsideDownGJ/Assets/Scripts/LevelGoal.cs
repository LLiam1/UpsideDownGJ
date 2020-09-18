using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private GameController controller;

    void Start()
    {
        controller = FindObjectOfType<GameController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            controller.NextLevel();
        }
    }
}
