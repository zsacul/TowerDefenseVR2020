using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSoundAttachment : BaseSoundAttachment
{
    override public void Play() 
    {
        AudioManager.I.playSoundOnce(clipToPlay, this.gameObject, mixer);
       // AudioManager.I.fadeOut(this.gameObject, 1.1f);
    }
}
