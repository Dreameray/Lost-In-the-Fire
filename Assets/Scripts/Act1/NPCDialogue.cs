using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    public string[] dialogueLines;

    public bool[] autoProgressLines;
    public bool[] endDialogueLines; // Indicates if the line ends the dialogue

    public float autoProgressDelay = 1.5f; // Delay before automatically progressing to the next line

    public float typingSpeed = 0.05f; // Speed at which text appears

    public AudioClip voiceSound;
    public float voicePitch;

    public DialogueChoice[] choices; // Array of choices for the dialogue
    
    public TMP_FontAsset customFont;


}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; // Index of the dialogue this choice is associated with
    public string[] choices; // Text for each choice
    public int[] nextDialogueIndexes; // Indices of the next dialogues for each choice
}
