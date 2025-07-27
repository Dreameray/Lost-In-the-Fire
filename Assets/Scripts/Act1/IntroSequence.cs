using UnityEngine;
using System.Collections;

public class IntroQuoteSequence : MonoBehaviour
{
    [Header("Canvas Groups")]
    public CanvasGroup quoteGroup;
    public CanvasGroup authorGroup;

    [Header("Timing")]
    public float fadeDuration = 2f;      // fade time
    public float quotePauseTime = 1.5f;  // how long only quote stays before author
    public float authorDisplayTime = 2.5f; // how long both stay before fade out

    void Start()
    {
        // Start the sequence
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        Debug.Log("Intro started");

        // 1. Fade in quote
        Debug.Log("Fading in quote...");
        yield return StartCoroutine(FadeCanvasGroup(quoteGroup, 0, 1, fadeDuration));

        // 2. Pause with only the quote
        Debug.Log("Quote visible, waiting before author");
        yield return new WaitForSeconds(quotePauseTime);

        // 3. Fade in author text
        Debug.Log("Fading in author...");
        yield return StartCoroutine(FadeCanvasGroup(authorGroup, 0, 1, fadeDuration));

        // 4. Keep both on screen
        Debug.Log("Both visible, wait before fade out");
        yield return new WaitForSeconds(authorDisplayTime);

        // 5. Fade both out together
        Debug.Log("Fading out both...");
        StartCoroutine(FadeCanvasGroup(quoteGroup, 1, 0, fadeDuration));
        yield return StartCoroutine(FadeCanvasGroup(authorGroup, 1, 0, fadeDuration));

        // 6. Done
        Debug.Log("Intro finished → now go to menu");
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;
        cg.alpha = start;
        while (time < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }
}
