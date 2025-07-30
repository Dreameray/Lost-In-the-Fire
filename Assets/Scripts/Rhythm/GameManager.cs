using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this line
using UnityEngine.Video; // ✅ Required for VideoPlayer
using Unity.VisualScripting;
using UnityEngine.UI; // ✅ Required for UI elements
using System.Collections; 


public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public VideoPlayer videoPlayer; // ✅ Assign in the Inspector later

    public static SongData selectedSong; // ✅ Store it for global access



    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 50;
    public int scorePerGoodNote = 100;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public TextMeshProUGUI percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    [SerializeField] public GameObject loadingPanel; // assign in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        scoreText.text = "0";
        currentMultiplier = 1;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
        }

        // Remove this - don't always show selection
        // if (SongSelectionManager.instance != null)
        // {
        //     SongSelectionManager.instance.ShowSongSelection();
        // }
    }


    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;
                theMusic.Play();

            }
        }
        else
        {
            // ✅ Only trigger results when song has truly ended
            if (!resultsScreen.activeInHierarchy &&
                theMusic.clip != null &&
                theMusic.time >= theMusic.clip.length - 0.1f)
            {
                resultsScreen.SetActive(true);

                normalsText.text = "" + normalHits;
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = "" + missedHits;

                float totalHit = normalHits + goodHits + perfectHits;
                float percentHit = 0f;

                if (totalNotes > 0)
                {
                    percentHit = (totalHit / totalNotes) * 100f;
                }
                else
                {
                    percentHit = 0f;
                }

                percentHit = Mathf.Clamp(percentHit, 0f, 100f);
                percentHitText.text = percentHit.ToString("F1") + "%";


                string rankVal = "F";

                if (percentHit > 90 && currentMultiplier >= 10)
                    rankVal = "SS";
                else if (percentHit > 95)
                    rankVal = "S";
                else if (percentHit > 85)
                    rankVal = "A";
                else if (percentHit > 70)
                    rankVal = "B";
                else if (percentHit > 55)
                    rankVal = "C";
                else if (percentHit > 40)
                    rankVal = "D";

                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();

                // Add a button to your Results Screen in Unity Inspector
                // that calls this method when clicked
                Button continueButton = resultsScreen.GetComponentInChildren<Button>();
                continueButton.onClick.AddListener(ReturnToSongSelection);
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Note Hit!");

        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierTracker >= multiplierThresholds[currentMultiplier - 1])
            {
                currentMultiplier++;
                multiplierTracker = 0;
            }
        }

        multiText.text = "x" + currentMultiplier.ToString();
        scoreText.text = currentScore.ToString();
    }

    public void NormalHit()
    {
        Debug.Log("Normal Hit!");
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        Debug.Log("Good Hit!");
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        Debug.Log("Perfect Hit!");
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Note Missed!");

        currentMultiplier = 1;
        multiplierTracker = 0;

        multiText.text = "x" + currentMultiplier.ToString();

        missedHits++;
    }

    // Add method to return to song selection
    public void ReturnToSongSelection()
    {
        resultsScreen.SetActive(false);
        if (SongSelectionManager.instance != null)
        {
            SongSelectionManager.instance.ShowSongSelection();
        }
        else
        {
            Debug.LogError("SongSelectionManager instance is null! Check if it exists in the scene.");
        }
    }

    public void StartNewSong(SongData song)
    {

        selectedSong = song;

        // Reset all stats
        currentScore = 0;
        normalHits = 0;
        goodHits = 0;
        perfectHits = 0;
        missedHits = 0;
        totalNotes = 0;
        currentMultiplier = 1;
        multiplierTracker = 0;


        // Reset UI
        scoreText.text = "0";
        multiText.text = "x1";
        resultsScreen.SetActive(false);

        // 🎵 Set up new audio
        theMusic.clip = song.audioClip;
        theMusic.Stop(); // Reset to beginning

        // 🎞️ Set up new video
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Reset any previous video
            videoPlayer.clip = song.videoClip;

            // This ensures audio and video start from the same time
            videoPlayer.time = 0;
        }

        // 🧠 Apply note settings
        theBS.beatTempo = song.bpm;
        theBS.RecalculateSpeed();
        theBS.ResetPosition();

        var noteSpawner = FindObjectOfType<NoteSpawner>();
        if (noteSpawner != null)
        {
            noteSpawner.spawnInterval = song.noteSpawnInterval;
            noteSpawner.ResetSpawner();
        }

        // ✅ Start gameplay
        startPlaying = true;
        theBS.hasStarted = true;

        if (videoPlayer != null)
            videoPlayer.Play(); // Start video

        theMusic.Play(); // Start audio
    }
    

    public IEnumerator LoadAndStartGame(SongData song)
    {
        loadingPanel.SetActive(true); // show UI
        TMP_Text preloadTMP = new GameObject("TMPPreload").AddComponent<TextMeshProUGUI>();
        preloadTMP.text = "TMP Init";
        preloadTMP.gameObject.SetActive(false);

        videoPlayer.clip = song.videoClip;
        theMusic.clip = song.audioClip;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        yield return new WaitForSeconds(1.5f); // short buffer
        loadingPanel.SetActive(false);
        StartNewSong(song);
    }



}
