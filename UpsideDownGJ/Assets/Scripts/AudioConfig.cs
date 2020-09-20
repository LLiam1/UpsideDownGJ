using System;
using UnityEngine;

[Serializable]
public class AudioConfig
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 0.07f;

    [Range(-3, 3)]
    public float pitch = 1f;
}
