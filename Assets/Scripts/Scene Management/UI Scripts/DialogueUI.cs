using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace Proto.SceneManagement.UI {
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] Button quitButton;



        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(() => playerConversant.Quit());
            UpdateUI();
        }


        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive()) {return;}

            AIResponse.SetActive(!playerConversant.IsChoosing());
            //AIResponse.SetActive(true); // AI response always show even when player is choosing
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
                //nextButton.gameObject.SetActive(!playerConversant.HasNext()); // disable Next Button when player is choosing
            }
            else {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.GetText();
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(choice);
                });
            }
        }

    }
}
