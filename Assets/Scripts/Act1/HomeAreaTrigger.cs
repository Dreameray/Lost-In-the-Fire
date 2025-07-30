using UnityEngine;
using System.Collections;

public class HomeAreaTrigger : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private bool testMode = false; // Add this line

    private void Start()
    {
        // Find LevelLoader in scene if not assigned
        if (levelLoader == null)
        {
            levelLoader = FindObjectOfType<LevelLoader>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (testMode)
            {
                // Skip quest check in test mode
                Debug.Log("Test mode: Triggering transition directly");
                StartCoroutine(TransitionAfterDelay());
                return;
            }

            var quest = QuestManager.I.ActiveQuest;
            
            if (quest != null && quest.Title == "Return Home" && !quest.IsComplete)
            {
                Debug.Log("Player entered home area, completing quest.");
                QuestManager.I.AddProgress(1);
                
                // Start transition after quest completion
                StartCoroutine(TransitionAfterDelay());
            }
        }
    }

    private IEnumerator TransitionAfterDelay()
    {
        // Give a small delay to let quest completion register
        yield return new WaitForSeconds(1.5f);
        
        // Transition to the next scene
        if (levelLoader != null)
        {
            levelLoader.LoadNextLevel();
        }
        else
        {
            Debug.LogError("LevelLoader not found!");
        }
    }
}