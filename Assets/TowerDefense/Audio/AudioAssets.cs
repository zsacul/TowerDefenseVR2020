using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*This is just a container for sounds and mixers
used only if one needs to acces some sound asset from code.
AudioClips start with s, AudioMixers start with m, please follow this rule.
*/


public class AudioAssets : MonoBehaviour
{

    public AudioClip sTowerShotBlaster;


    public AudioMixerGroup mProjectilesMaster;
}
