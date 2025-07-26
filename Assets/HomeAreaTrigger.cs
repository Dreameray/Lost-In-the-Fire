using UnityEngine;

public class HomeAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered home area: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            var quest = QuestManager.I.ActiveQuest;
            Debug.Log("ActiveQuest: " + (quest == null ? "null" : quest.Title));
            if (quest != null)
            {
                Debug.Log("ActiveQuest Title: " + quest.Title);
                Debug.Log("IsComplete: " + quest.IsComplete);
            }
            if (quest != null && quest.Title == "Return Home" && !quest.IsComplete)
            {
                Debug.Log("Player entered home area, completing quest.");
                QuestManager.I.AddProgress(1);
            }
        }
    }
}