using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Volume2D : MonoBehaviour
{
    public Transform listenerTransform;
    public AudioSource audioSource;
    public float minDist=1;
    public float maxDist=20; //keep in mind camera has z distance as well

    [SerializeField] float audioVolMax = 0.5f;
 
    void Update()
    {
        float dist = Vector3.Distance(transform.position, listenerTransform.position);
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

        audioSource.volume = volScale*audioVolMax;
    }
}