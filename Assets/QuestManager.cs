// QuestManager.cs
using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager I { get; private set; }
    public Quest ActiveQuest { get; private set; }

    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestUpdated;
    public event Action<Quest> OnQuestCompleted;

    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    public void StartQuest(Quest quest)
    {
        ActiveQuest = quest;
        OnQuestStarted?.Invoke(quest);
    }

    public void AddProgress(int amount = 1)
    {
        if (ActiveQuest == null || ActiveQuest.IsComplete) return;
        ActiveQuest.CurrentCount += amount;
        OnQuestUpdated?.Invoke(ActiveQuest);

        if (ActiveQuest.IsComplete)
            CompleteQuest();
    }

    private void CompleteQuest()
    {
        OnQuestCompleted?.Invoke(ActiveQuest);
        ActiveQuest = null;
    }
}
