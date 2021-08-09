using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // This script was created by Brackeys in this tutorial: 
    // https://www.youtube.com/watch?v=6OT43pvUyfY&lc=Ugiwk2y9Cy57dngCoAEC

    public AudioMixerGroup mixer;
    public Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("The Sound " + name + " could not be found!");
            return;
        }
        s.source.Play();
    }
}
