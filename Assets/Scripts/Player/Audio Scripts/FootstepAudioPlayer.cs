using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Proto.Audio {

public class FootstepAudioPlayer : MonoBehaviour
{
    [SerializeField] private FootstepAudioSamples defaultAudioSamples;
    [SerializeField] private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.movement.sqrMagnitude > 0.1f) {
            var samples =defaultAudioSamples;
            if (samples == null) return;
            var audio = samples.PickRandom();
            //audioSource.PlayOneShot(audio);
            //audioSource.clip = audio;
            //audioSource.Play();
        } 
        
    }


}

}

