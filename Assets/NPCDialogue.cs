using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    public string[] dialogueLines;

    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f; // Delay before automatically progressing to the next line

    public float typingSpeed = 0.05f; // Speed at which text appears

    public AudioClip voiceSound;
    public float voicePitch;
   
}
