using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Audio;
public class Blacksmith : MonoBehaviour
{

    private GenericAudioPlayer genericAudioPlayer;
    

    private void Awake() {
        genericAudioPlayer = GetComponentInChildren<GenericAudioPlayer>();
    }

    
    public void HammerAnvilEvent() {
        genericAudioPlayer.PlayRandomAudioClip();
    }

}
