using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    
    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAnimator.Play("DefaultCamera");
    }

    public void SwitchState(bool defaultCamera) {
        if (defaultCamera) {
            myAnimator.Play("DefaultCamera");
        } else {
            myAnimator.Play("IndoorCamera");
        }
    }

}
