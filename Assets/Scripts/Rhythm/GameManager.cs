using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this line

public class GameManager : MonoBehaviour
{

    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        scoreText.text = "Score: 0";
        currentMultiplier = 1;

        // Set up the selected song
        if (SongSelectionManager.selectedSong != null)
        {
            theMusic.clip = SongSelectionManager.selectedSong.audioClip;
            theBS.beatTempo = SongSelectionManager.selectedSong.bpm;
            FindObjectOfType<NoteSpawner>().spawnInterval = SongSelectionManager.selectedSong.noteSpawnInterval;
        }
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
            if (!theMusic.isPlaying && !resultsScreen.activeInHierarchy)
            {
                resultsScreen.SetActive(true);

                normalsText.text = "" + normalHits;
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = "" + missedHits;

                float totalHit = normalHits + goodHits + perfectHits;
                float percentHit = (totalHit / totalNotes) + 100f;

                percentHitText.text = percentHit.ToString("F1") + "%";

                string rankVal = "F";

                if (percentHit > 40)
                {
                    rankVal = "D";
                    if (percentHit > 55)
                    {
                        rankVal = "C";
                        if (percentHit > 70)
                        {
                            rankVal = "B";
                            if (percentHit > 85)
                            {
                                rankVal = "A";
                                if (percentHit > 95)
                                {
                                    rankVal = "S";
                                }
                            }
                        }
                    }
                }

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

        multiText.text = "Multiplier: x" + currentMultiplier.ToString();
        scoreText.text = "Score: " + currentScore.ToString();
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

        multiText.text = "Multiplier: x" + currentMultiplier.ToString();

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
        scoreText.text = "Score: 0";
        multiText.text = "Multiplier: x1";
        resultsScreen.SetActive(false);
        
        // Set up new song
        theMusic.clip = song.audioClip;
        theBS.beatTempo = song.bpm;
        theBS.RecalculateSpeed(); // Use the new public method instead
        theBS.ResetPosition();
        
        // Update note spawn interval
        var noteSpawner = FindObjectOfType<NoteSpawner>();
        if (noteSpawner != null)
        {
            noteSpawner.spawnInterval = song.noteSpawnInterval;
            noteSpawner.ResetSpawner(); // Add this method to NoteSpawner
        }
        
        // Auto-start the song and gameplay
        startPlaying = true;
        theBS.hasStarted = true;
        theMusic.Play();
    }
}
