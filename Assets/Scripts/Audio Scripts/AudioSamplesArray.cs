using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Audio {

    [CreateAssetMenu(fileName = "AudioSamplesArray", menuName = "Prototype/AudioSamplesArray")]
    public class AudioSamplesArray : ScriptableObject
    {
 
        #region Fields
 
        [SerializeField]
        private AudioClip[] audioClips;
 
        #endregion
 
        #region Properties
 
        public int Count { get { return audioClips.Length; } }
 
        public AudioClip this[int index]
        {
            get { return audioClips[index]; }
        }
 
        #endregion
 
        #region Methods
 
        public AudioClip PickRandom()
        {
            int index = Random.Range(0,Count);
            return audioClips[index];
        }
 
        #endregion
 
    }

}
