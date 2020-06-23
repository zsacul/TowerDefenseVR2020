using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingSoundAttachment : BaseSoundAttachment
{
    [SerializeField]
    bool playOnAwake = false;

    AudioSource src;

    private void Start()
    {
        if(playOnAwake)
        {
            Play();
        }
    }
    override public void Play()
    {
        AudioManager.Instance.PlayLooped(clipToPlay, this.gameObject, mixer);
        src = GetComponent<AudioSource>();
    }

    public void Stop()
    {
        if (src != null && src.isPlaying)
            AudioManager.Instance.FadeOut(this.gameObject, 0.6f);
    }
}
