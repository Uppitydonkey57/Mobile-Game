using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    // This script was created by Brackeys in this tutorial: 
    // https://www.youtube.com/watch?v=6OT43pvUyfY&lc=Ugiwk2y9Cy57dngCoAEC
    
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
