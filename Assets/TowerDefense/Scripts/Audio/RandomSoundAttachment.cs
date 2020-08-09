using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RandomSoundAttachment : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> clips;

    [SerializeField]
    Vector3 locationOverride;

    [SerializeField]
    protected AudioMixerGroup mixer;

    Vector3 soundLocation;
    private void Start()
    {

    }
    public void Play()
    {
        if (locationOverride == Vector3.zero)
        {
            soundLocation = this.transform.position;
        }
        else
        {
            soundLocation = locationOverride;
        }

        
        AudioManager.Instance.PlayOnceAtLocation(clips[Random.Range(0, clips.Count)], soundLocation, mixer);
    }
}
