using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Proto.Audio {

public class GenericAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSamplesArray defaultAudioSamples;
    [SerializeField] private AudioSamplesArray SecondaryAudioSamples;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip SecondaryAudio;
    [SerializeField] private AudioClip PrimaryAudio;
    [SerializeField] private float AudioClipVol = 0.3f;
    [SerializeField] private float AudioSourceVol = 0.5f;

    public float minDist=1;
    public float maxDist=15;
    private float relativeClipVolume = 0f;
    
    private float relativeSourceVolume = 0f;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        float volScale = 0f;
        if(dist < minDist)
        {
            volScale = 1;
        }
        else if(dist > maxDist)
        {
            volScale = 0;
        }
        else
        {
            volScale = 1 - ((dist - minDist) / (maxDist - minDist));
        }

        relativeClipVolume = volScale*AudioClipVol;
        relativeSourceVolume = volScale*AudioSourceVol;
    }
    
    public void PlayRandomAudioClip() {
        var samples = defaultAudioSamples;
        if (samples == null) return;
        var audio = samples.PickRandom();
        audioSource.PlayOneShot(audio, relativeClipVolume);        
    }

    public void PlayRandomSecondaryClip() {
        var samples = SecondaryAudioSamples;
        if (samples == null) return;
        var audio = samples.PickRandom();
        audioSource.PlayOneShot(audio, relativeClipVolume);        
    }

    public void PlaySecondaryAudio() {
        audioSource.PlayOneShot(SecondaryAudio, relativeSourceVolume);
    }
    public void PlayPrimaryAudio() {
        audioSource.PlayOneShot(PrimaryAudio, relativeSourceVolume);
    }

}

}

