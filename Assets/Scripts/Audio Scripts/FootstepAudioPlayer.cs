using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Proto.Audio {

public class FootstepAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSamplesArray defaultAudioSamples;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip RollingAudio;
    [SerializeField] private float walkAudioVol = 0.2f;
    [SerializeField] private float runAudioVol = 0.4f;
    
    
    public void PlayFootstepAudioClip() {
        var samples =defaultAudioSamples;
        if (samples == null) return;
        var audio = samples.PickRandom();
        if (PlayerController.Instance.sprint) {
            audioSource.PlayOneShot(audio, runAudioVol);
        } else {
            audioSource.PlayOneShot(audio, walkAudioVol);
        }
        
    }

    public void PlayRollAudio() {
        audioSource.PlayOneShot(RollingAudio);
    }

}

}

