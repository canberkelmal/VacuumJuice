using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    } 

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        /* If not playing.
        if (!s.source.isPlaying)
        {
            s.source.Play(); 
            Debug.Log(name + " is _REplayed!");
        }*/

        if (s.source.isPlaying)
        {
            s.source.time = 0;
        }
        else
        {
            s.source.Play();
        }
    } 

    public void SetVolume(float level)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = level;
        }
    }

}
