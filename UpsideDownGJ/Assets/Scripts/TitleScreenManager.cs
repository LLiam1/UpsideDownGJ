using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneFader))]
public class TitleScreenManager : MonoBehaviour
{
    public GameObject fadeScreen;
    private SceneFader fader;

    public GameObject titleScreen;
    public GameObject levelScreen;
    public GameObject creditsScreen;
    public GameObject settingsScreen;

    public Toggle soundEffectsToggle;
    public Toggle musicToggle;

    private int muteMusic;
    private int muteSound;

    void Start()
    {
        fader = GetComponent<SceneFader>();
        fadeScreen.SetActive(false);

        DisplayTitle();

        if(!PlayerPrefs.HasKey("MuteSounds") && !PlayerPrefs.HasKey("MuteMusic"))
        {
            Debug.Log("Player Prefs Set!");
            PlayerPrefs.SetInt("MuteMusic", 0);
            PlayerPrefs.SetInt("MuteSounds", 0);
        }

        musicToggle.isOn = GetPlayerPref("MuteMusic");
        soundEffectsToggle.isOn = GetPlayerPref("MuteSounds");

    }

    private void Update()
    {

        if (musicToggle.isOn == true) { muteMusic = 1; } else { muteMusic = 0; }
        if (soundEffectsToggle.isOn == true) { muteSound = 1; } else { muteSound = 0; }

        PlayerPrefs.SetInt("MuteMusic", muteMusic);
        PlayerPrefs.SetInt("MuteSounds", muteSound);
    }

    public void ToScene(string sceneName)
    {
        SoundManager.i.PlaySound("button_click");
        //fadeScreen.SetActive(true);
        StartCoroutine(fader.FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }

    public void DisplayTitle()
    {

        titleScreen.SetActive(true);
        levelScreen.SetActive(false);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }

    public void DisplayCredits()
    {

        titleScreen.SetActive(false);
        levelScreen.SetActive(false);
        creditsScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void DisplayerLevelSelector()
    {
        titleScreen.SetActive(false);
        levelScreen.SetActive(true);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }

    public void DiplaySettings()
    {
        titleScreen.SetActive(false);
        levelScreen.SetActive(false);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public bool GetPlayerPref(string pref)
    {
        int prefVal = PlayerPrefs.GetInt(pref);

        if(prefVal == 1)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
