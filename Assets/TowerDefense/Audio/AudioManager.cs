﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    //singleton, use this to acces anything (I short for Instance)
    [HideInInspector]
    public static AudioManager I;

    //container for audio clips and mixers
    [HideInInspector]
    public AudioAssets Sounds;

    private void Awake()
    {
        if (I != null)
            GameObject.Destroy(I);
        else
            I = this;

        DontDestroyOnLoad(this);

        Sounds = GetComponent<AudioAssets>();
    }

    public void PlaySound(AudioClip soundToPlay, GameObject originOfSound, AudioMixerGroup mixer, bool isLooped = false)
    {
        if(originOfSound.GetComponent<AudioSource>() == null)
        {
            AudioSource newSource = originOfSound.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.spatialBlend = 1.0f;
        }
       
        AudioSource src = originOfSound.GetComponent<AudioSource>();
        src.loop = isLooped;
        src.outputAudioMixerGroup = mixer;
        src.clip = soundToPlay;
        src.Play();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
