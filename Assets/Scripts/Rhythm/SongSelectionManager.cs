using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class SongSelectionManager : MonoBehaviour
{
    public static SongSelectionManager instance;
    public static SongData selectedSong;

    // Add these at the top of the class
    public static bool isReplay = false;
    [SerializeField] private SongData defaultSong; // Assign your "Fairytale" song in inspector

    [Header("UI References")]
    public GameObject songSelectionPanel;    // Assign the main panel
    public Transform buttonContainer;         // Assign the button container
    public Button songButtonPrefab;          // Assign your button prefab
    
    [Header("Optional UI References")]
    public Text titleText;                   // Reference to "Select a song" text if needed
    public Image backgroundImage;            // Reference to panel background if needed

    [Header("Song Data")]
    public List<SongData> availableSongs;    // Your list of songs

    public Button finishGameButton; // Button to finish the game

    [SerializeField] private LevelLoader levelLoader;  // Add this field

    void Awake()
    {
        Debug.Log($"SongSelectionManager Awake - isReplay: {isReplay}");
        if (instance == null)
        {
            instance = this;
            CreateSongButtons();
            SetupFinishButton();

            if (songSelectionPanel != null)
            {
                // Show panel only if it's a replay
                songSelectionPanel.SetActive(isReplay);
                
                // If it's first time, start with default song
                if (!isReplay && defaultSong != null)
                {
                    // Add delay to ensure GameManager is ready
                    StartCoroutine(StartWithDefaultSong());
                }
            }
            else
            {
                Debug.LogError("Song Selection Panel is not assigned!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StartWithDefaultSong()
    {
        // Wait for next frame to ensure GameManager is initialized
        yield return new WaitForEndOfFrame();
        
        if (GameManager.instance != null && defaultSong != null)
        {
            SelectSong(defaultSong);
            Debug.Log($"Starting with default song: {defaultSong.songName}");
        }
        else
        {
            Debug.LogError("GameManager not ready or defaultSong not assigned!");
        }
    }

    void CreateSongButtons()
    {
        if (buttonContainer == null)
        {
            Debug.LogError("Button Container is not assigned!");
            return;
        }

        // Clear existing buttons first
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"Creating buttons for {availableSongs.Count} songs"); // Added debug

        foreach (var song in availableSongs)
        {
            if (song == null)
            {
                Debug.LogError("Null song data found in availableSongs list!");
                continue;
            }

            // Debug song data
            Debug.Log($"Processing song: Name='{song.songName}', BPM={song.bpm}, Difficulty='{song.difficulty}'");

            // Create button
            Button newButton = Instantiate(songButtonPrefab, buttonContainer);
            
            // Get the Text component (make sure it exists on prefab)
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText == null)
            {
                Debug.LogError($"Button prefab '{songButtonPrefab.name}' is missing TMP_Text component as child!");
                continue;
            }

            // Set button properties
            newButton.name = $"Button_{song.songName}";
            buttonText.text = $"{song.songName}\n{song.difficulty}\nBPM: {song.bpm}";
            
            // Add click handler
            newButton.onClick.AddListener(() => SelectSong(song));

            Debug.Log($"Created button for song: {song.songName}");
        }
    }

    void SetupFinishButton()
    {
        if (finishGameButton != null)
        {
            finishGameButton.onClick.AddListener(OnFinishGameClicked);
            
            // Find LevelLoader if not assigned
            if (levelLoader == null)
            {
                levelLoader = FindObjectOfType<LevelLoader>();
            }
        }
        else
        {
            Debug.LogError("Finish Game Button is not assigned!");
        }
    }

    void OnFinishGameClicked()
    {
        if (levelLoader != null)
        {
            levelLoader.LoadNextLevel(); // This will transition to EndCredits scene
        }
        else
        {
            Debug.LogError("LevelLoader not found!");
        }
    }

    public void SelectSong(SongData song)
    {
        Debug.Log($"Selecting song: {song.songName}");
        selectedSong = song;
        songSelectionPanel.SetActive(false);

        // Reset BeatScroller position
        GameManager.instance.theBS.transform.position = Vector3.zero;

        //GameManager.instance.StartNewSong(song);
        // 🧠 Use coroutine instead of direct start
        StartCoroutine(GameManager.instance.LoadAndStartGame(song));

    }

    public void ShowSongSelection()
    {
        if (songSelectionPanel != null)
        {
            songSelectionPanel.SetActive(true);
            Debug.Log("Showing song selection panel");
        }
        else
        {
            Debug.LogError("Song Selection Panel is null!");
        }
    }
}