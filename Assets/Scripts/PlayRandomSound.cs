using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;

    [SerializeField] private AudioSource source;

    private void Start()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
    }

    public void PlaySound()
    {
        source.PlayOneShot(sounds[Random.Range(0, sounds.Length)]); 
    }
}
