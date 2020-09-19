using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneFader))]
public class UIManager : MonoBehaviour
{
    public string nextLevelName;

    public GameObject fadeScreen;
    private SceneFader fader;

    void Start()
    {
        fader = GetComponent<SceneFader>();

        fadeScreen.SetActive(false);
        StartCoroutine(fader.Fade(SceneFader.FadeDirection.Out));
    }
    public void NextLevel()
    {
        StartCoroutine(fader.FadeAndLoadScene(SceneFader.FadeDirection.In, nextLevelName));
    }

    public void OpenLevelSelector()
    {

    }

    public void DisplayCredits()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
