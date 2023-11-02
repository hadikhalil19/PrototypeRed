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
    
    public void PlayFootstepAudioClip() {
        var samples =defaultAudioSamples;
        if (samples == null) return;
        var audio = samples.PickRandom();
        if (PlayerController.Instance.sprint) {
            audioSource.PlayOneShot(audio, 0.7f);
        } else {
            audioSource.PlayOneShot(audio, 0.3f);
        }
        
    }

    public void PlayRollAudio() {
        audioSource.PlayOneShot(RollingAudio);
    }

}

}

