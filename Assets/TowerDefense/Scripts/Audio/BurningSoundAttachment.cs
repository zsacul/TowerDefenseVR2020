using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSoundAttachment : BaseSoundAttachment
{
    AudioSource src;
    override public void Play()
    {
        AudioManager.Instance.PlayLooped(clipToPlay, this.gameObject, mixer);
        src = GetComponent<AudioSource>();
        src.rolloffMode = AudioRolloffMode.Linear;
        src.maxDistance = 6.0f;
    }

    public void Stop()
    {
        AudioManager.Instance.FadeOut(this.gameObject, 0.6f);
    }
}
