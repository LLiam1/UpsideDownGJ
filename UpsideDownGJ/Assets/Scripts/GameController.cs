using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneFader))]
public class GameController : MonoBehaviour
{
    public string nextLevelName;
    public bool switched;

    public GameObject fadeScreen;

    private Rigidbody2D[] rigidBodies;
    private SceneFader fader;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = FindObjectsOfType<Rigidbody2D>();
        switched = false;
        fader = GetComponent<SceneFader>();

        fadeScreen.SetActive(true);
        StartCoroutine(fader.Fade(SceneFader.FadeDirection.Out));
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

    public void NextLevel()
    {
        StartCoroutine(fader.FadeAndLoadScene(SceneFader.FadeDirection.In, nextLevelName));
    }
}
