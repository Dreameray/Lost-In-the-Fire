using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; } //Singleton instance

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClickAction)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClickAction);
    }
    

}
