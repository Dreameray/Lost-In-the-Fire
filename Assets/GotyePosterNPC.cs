// GotyePosterNPC.cs
using UnityEngine;

public class GotyePosterNPC : MonoBehaviour
{
    public AudioClip gotyeSong;
    private AudioSource audioSrc;
    private bool started = false;

    private void Start()
    {
        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.loop = true;
    }

    // Call this when your dialogue controller signals “done”
    public void OnDialogueComplete()
    {
        if (started) return;
        started = true;

        // 1) Start Quest 1
        Quest q1 = new Quest("Yummy Yummy", "Find 3 yarns", 3);
        QuestManager.I.StartQuest(q1);

        // 2) Play the song
        audioSrc.clip = gotyeSong;
        audioSrc.Play();
    }
}
