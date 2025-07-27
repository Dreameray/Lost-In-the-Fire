using UnityEngine;

// Quest.cs
public class Quest
{
    public string Title;
    public string Description;
    public int CurrentCount;
    public int RequiredCount;
    public bool IsComplete => CurrentCount >= RequiredCount;

    public Quest(string title, string description, int requiredCount = 0)
    {
        Title         = title;
        Description   = description;
        RequiredCount = requiredCount;
        CurrentCount  = 0;
    }
}

