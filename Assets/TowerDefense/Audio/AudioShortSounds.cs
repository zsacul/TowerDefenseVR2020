using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioShortSounds : MonoBehaviour
{
    [SerializeField]
    AudioClip clipToPlay;

    [SerializeField]
    AudioMixerGroup mixer;

    public void Play()
    {
        AudioManager.I.PlaySound(clipToPlay, this.gameObject, mixer);
    }
}
