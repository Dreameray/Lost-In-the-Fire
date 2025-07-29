// QuestUI.cs
using System.Collections;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject questLogPanel;
    public CanvasGroup questLogCanvas;    // drag QuestLogPanel’s CanvasGroup here
    public TMP_Text   titleText;
    public TMP_Text   descText;

    public GameObject newQuestText;
    public CanvasGroup newQuestCanvas;    // drag NewQuestText’s CanvasGroup here

    [Header("Timings")]
    public float bannerFadeDuration    = 1f;  // seconds to fade in/out banner
    public float newQuestBannerDuration = 5f; // how long banner stays fully visible
    public float delayBeforeLogShow     = 2f; // wait after banner fades out
    public float logFadeDuration       = 1f;  // seconds to fade in log
    public float panelHideDelay        = 3f;  // after final quest completes

    private bool secondQuestStarted = false;
    
    [Header("Quest Music Control")]
    public AudioSource questMusicSource;
    public AudioClip betweenQuestSong;


    private void Start()
    {
        // hide both at start
        newQuestText.SetActive(false);
        questLogPanel.SetActive(false);

        if (QuestManager.I == null)
        {
            Debug.LogError("QuestManager missing!");
            enabled = false;
            return;
        }
        QuestManager.I.OnQuestStarted += OnQuestStarted;
        QuestManager.I.OnQuestUpdated += OnQuestUpdated;
        QuestManager.I.OnQuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        if (QuestManager.I != null)
        {
            QuestManager.I.OnQuestStarted   -= OnQuestStarted;
            QuestManager.I.OnQuestUpdated   -= OnQuestUpdated;
            QuestManager.I.OnQuestCompleted -= OnQuestCompleted;
        }
    }

    private void OnQuestStarted(Quest q)
    {
        // prepare texts
        titleText.text = q.Title;
        descText.text  = FormatDesc(q);

        // kick off the sequence
        StopAllCoroutines();
        StartCoroutine(BannerThenLogSequence());
    }

    

    private IEnumerator BannerThenLogSequence()
    {
        // 1) Fade in NEW QUEST banner
        newQuestText.SetActive(true);
        yield return FadeCanvasGroup(newQuestCanvas, 0f, 1f, bannerFadeDuration);

        // 2) Wait while banner is fully visible
        yield return new WaitForSeconds(newQuestBannerDuration);

        // 3) Fade out banner
        yield return FadeCanvasGroup(newQuestCanvas, 1f, 0f, bannerFadeDuration);
        newQuestText.SetActive(false);

        // 4) Delay before showing quest log
        yield return new WaitForSeconds(delayBeforeLogShow);

        // 5) Fade in quest log panel
        questLogPanel.SetActive(true);
        yield return FadeCanvasGroup(questLogCanvas, 0f, 1f, logFadeDuration);
    }

    private void OnQuestUpdated(Quest q)
    {
        descText.text = FormatDesc(q);
    }

    private void OnQuestCompleted(Quest q)
    {
        Debug.Log($"Quest completed: {q.Title}");

        titleText.text = $"{q.Title} Completed!";
        descText.text  = "";

        if (q.Title == "Yummy Yummy" && !secondQuestStarted)
        {
            secondQuestStarted = true;
            StartCoroutine(DelayedStartSecondQuest());
        }

        // if it’s your final quest (RequiredCount==0), fade out the panel after a delay
        if (q.RequiredCount == 0)
            StartCoroutine(HideLogWithFade());
    }

    private IEnumerator HideLogWithFade()
    {
        yield return new WaitForSeconds(panelHideDelay);
        yield return FadeCanvasGroup(questLogCanvas, 1f, 0f, logFadeDuration);
        questLogPanel.SetActive(false);
    }

    private IEnumerator DelayedStartSecondQuest()
    {
        // Wait for the completion message and fade out
        yield return new WaitForSeconds(panelHideDelay + logFadeDuration);

        // Now start the second quest
        Quest q2 = new Quest("Return Home", "", 1);
        QuestManager.I.StartQuest(q2);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float dur)
    {
        float t = 0f;
        cg.alpha = start;
        while (t < dur)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / dur);
            yield return null;
        }
        cg.alpha = end;
    }

    private string FormatDesc(Quest q)
    {
        // Hide progress for "Return Home"
        if (q.Title == "Return Home")
            return ""; // or return ""; if you want it totally blank

        // Default: show progress
        if (q.RequiredCount > 0)
            return $"{q.Description} {q.CurrentCount}/{q.RequiredCount}";
        else
            return q.Description;
    }
}
