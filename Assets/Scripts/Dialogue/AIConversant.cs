using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Dialogue {
public class AIConversant : MonoBehaviour
{

[SerializeField] Dialogue dialogue = null;

   void OnTriggerStay2D(Collider2D other) 
 {
    if(other.gameObject.tag == "Player")
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (dialogue == null) {return;}

            other.GetComponent<PlayerConversant>().StartDialogue(dialogue);

        }
        }
 } 


}
}
