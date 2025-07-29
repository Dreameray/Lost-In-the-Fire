using UnityEngine;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    /* ───────────────  UI REFERENCES  ─────────────── */
    [Header("UI Elements")]
    public CanvasGroup[] frenchLyrics;
    public CanvasGroup englishLyrics;
    public CanvasGroup companyName;
    public CanvasGroup unityLogo;
    public CanvasGroup songName;
    public CanvasGroup gameLogo;
    public CanvasGroup startButton;
    
    public CanvasGroup blackOverlay;

    [Header("Audio")]
    public AudioSource musicSource;

    public LevelLoader levelLoader;

    /* ───────────────  TIMINGS  ─────────────── */
    [Header("Timings")]
    [Tooltip("Each element’s absolute time (seconds) when a French lyric fades in")]
    public float[] frenchLyricTimings = { 5f, 8f, 11f, 14f };

    [Tooltip("Time (s) when all French lyrics fade OUT")]
    public float fadeOutFrenchLyricsAt = 25f;

    [Tooltip("Time (s) when English lyric block fades IN")]
    public float englishLyricsTime = 27f;

    [Tooltip("Seconds AFTER englishLyricsTime that song name fades IN")]
    public float songNameDelayAfterEnglish = 3f;

    [Tooltip("Time (s) when the game logo fades IN")]
    public float gameLogoTime = 36.5f;

    [Tooltip("Time (s) when the start button fades IN")]
    public float startButtonTime = 45f;

    /* Logo hold durations */
    [Header("Logo Timings")]
    public float companyLogoDuration = 2f;   // company logo visible this long
    public float unityLogoDuration   = 2f;   // unity logo visible this long

    /* INTERNAL STATE */
    private float timer;
    private bool hasStarted;
    private bool[] flag;                     // one‑shot triggers

    /* ───────────────  MONOBEHAVIOUR  ─────────────── */
    private void Start()
    {
        flag = new bool[20];

        // Reset all CanvasGroups to invisible
        Prepare(companyName); Prepare(unityLogo); Prepare(songName); Prepare(gameLogo);
        Prepare(startButton); Prepare(englishLyrics); Prepare(blackOverlay);
        foreach (var cg in frenchLyrics) Prepare(cg);

        blackOverlay.alpha = 1f;   // start with black screen

        /* Company logo sequence */
        StartCoroutine(FadeIn (companyName, 1f));
        StartCoroutine(FadeOut(companyName, 1f, companyLogoDuration - 1f));

        /* Unity logo sequence */
        StartCoroutine(FadeIn (unityLogo, 1f, companyLogoDuration));
        StartCoroutine(FadeOut(unityLogo, 1f, companyLogoDuration + unityLogoDuration - 1f));

        /* Begin music right after the two logos finish */
        Invoke(nameof(StartMusic), companyLogoDuration + unityLogoDuration);
    }

    private void Update()
    {
        if (!hasStarted) return;
        timer += Time.deltaTime;

        /* French lyrics IN */
        for (int i = 0; i < frenchLyrics.Length; i++)
            if (timer > frenchLyricTimings[i] && !flag[i])
            {
                StartCoroutine(FadeIn(frenchLyrics[i], 1f));
                flag[i] = true;
            }

        /* French lyrics OUT */
        if (timer > fadeOutFrenchLyricsAt && !flag[10])
        {
            foreach (var cg in frenchLyrics) StartCoroutine(FadeOut(cg, 1f));
            flag[10] = true;
        }

        /* English lyric IN */
        if (timer > englishLyricsTime && !flag[11])
        {
            StartCoroutine(FadeIn(englishLyrics, 1f));
            flag[11] = true;
        }

        /* Song name IN (delay after English) */
        if (timer > englishLyricsTime + songNameDelayAfterEnglish && !flag[12])
        {
            StartCoroutine(FadeIn(songName, 1f));
            flag[12] = true;
        }

        /* English + song name OUT exactly 3 s before game logo */
        if (timer > gameLogoTime - 3f && !flag[13])
        {
            StartCoroutine(FadeOut(englishLyrics, 1f));
            StartCoroutine(FadeOut(songName, 1f));
            flag[13] = true;
        }

        /* Game logo IN */
        if (timer > gameLogoTime && !flag[14])
        {
            StartCoroutine(FadeIn(gameLogo, 1f));
            flag[14] = true;
        }

        /* Start button IN */
        if (timer > startButtonTime && !flag[15])
        {
            StartCoroutine(FadeIn(startButton, 1f));
            flag[15] = true;
        }
    }

    public void OnStartGameClicked()
    {
        StartCoroutine(TransitionToNextScene());
    }

    /* ───────────────  HELPERS  ─────────────── */

    void Prepare(CanvasGroup cg)
    {
        if (cg == null) return;
        cg.alpha = 0f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    void StartMusic()
    {
        hasStarted = true;
        musicSource.Play();
    }

    IEnumerator TransitionToNextScene()
    {
        /* Fade out UI & music */
        StartCoroutine(FadeOut(gameLogo, 1f));
        StartCoroutine(FadeOut(startButton, 1f));
        musicSource.Stop();
        yield return new WaitForSeconds(1f);

        /* Disable all UI elements */
        foreach (var cg in frenchLyrics) cg.gameObject.SetActive(false);
        englishLyrics.gameObject.SetActive(false);
        songName.gameObject.SetActive(false);
        gameLogo.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        companyName.gameObject.SetActive(false);
        unityLogo.gameObject.SetActive(false);

        /* Start the level transition */
        levelLoader.LoadNextLevel();
    }

    IEnumerator FadeIn(CanvasGroup cg, float dur, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        for (float t = 0; t < dur; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(0, 1, t / dur);
            yield return null;
        }
        cg.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup cg, float dur, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        for (float t = 0; t < dur; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(1, 0, t / dur);
            yield return null;
        }
        cg.alpha = 0f;
    }
}
