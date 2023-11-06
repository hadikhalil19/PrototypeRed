using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Proto.Audio {

public class GenericAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSamplesArray defaultAudioSamples;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip SecondaryAudio;
    [SerializeField] private float AudioClipVol = 0.3f;
    [SerializeField] private float SecondaryAudioVol = 0.5f;

    public Transform listenerTransform;
    public float minDist=1;
    public float maxDist=20;

    // void Update()
    // {
    //     float dist = Vector3.Distance(transform.position, listenerTransform.position);
    //     float volScale = 0f;
    //     if(dist < minDist)
    //     {
    //         volScale = 1;
    //     }
    //     else if(dist > maxDist)
    //     {
    //         volScale = 0;
    //     }
    //     else
    //     {
    //         volScale = 1 - ((dist - minDist) / (maxDist - minDist));
    //     }

    //     audioSource.volume = volScale*audioVolMax;
    // }
    
    public void PlayRandomAudioClip() {
        var samples =defaultAudioSamples;
        if (samples == null) return;
        var audio = samples.PickRandom();
        audioSource.PlayOneShot(audio, AudioClipVol);        
    }

    public void PlaySecondaryAudio() {
        audioSource.PlayOneShot(SecondaryAudio, SecondaryAudioVol);
    }

}

}

