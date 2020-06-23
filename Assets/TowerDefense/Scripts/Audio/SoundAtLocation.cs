using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundAtLocation : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> clips;

    [SerializeField]
    Vector3 locationOverride;

    [SerializeField]
    protected AudioMixerGroup mixer;

    private void Start()
    {

    }
    public void Play(int clipNumber = 0)
    {
        if (locationOverride != Vector3.zero)
        {
            locationOverride = this.transform.position;
        }
        AudioManager.Instance.PlayOnceAtLocation(clips[clipNumber], locationOverride, mixer);
    }

}
