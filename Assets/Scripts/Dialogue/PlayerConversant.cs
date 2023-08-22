using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Proto.Dialogue {
    public class PlayerConversant : MonoBehaviour
    {
        
        //[SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        
        public bool isTalking  {get; private set;}

        public event Action onConversationUpdated;

        // IEnumerator Start()
        // {
        //     yield return new WaitForSeconds(2);
        //     StartDialogue(testDialogue);
        // }

        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
            isTalking = true;
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
            isTalking = false;
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }


        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }
            
            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
           return currentDialogue.GetPlayerChildren(currentNode);
        }

         public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            if (children.Count() == 0)
            {
                Quit();
                return;
            }
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

         private void TriggerEnterAction()
        {
            if (currentNode != null && currentNode.GetOnEnterAction() != "")
            {
                Debug.Log(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null && currentNode.GetOnExitAction() != "")
            {
                Debug.Log(currentNode.GetOnExitAction());
            }
        }
    }

}
