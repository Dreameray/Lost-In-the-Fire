using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SongSelectionManager : MonoBehaviour
{
    public static SongSelectionManager instance;
    public static SongData selectedSong;

    [Header("UI References")]
    public GameObject songSelectionPanel;    // Assign the main panel
    public Transform buttonContainer;         // Assign the button container
    public Button songButtonPrefab;          // Assign your button prefab
    
    [Header("Optional UI References")]
    public Text titleText;                   // Reference to "Select a song" text if needed
    public Image backgroundImage;            // Reference to panel background if needed

    [Header("Song Data")]
    public List<SongData> availableSongs;    // Your list of songs

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            CreateSongButtons();
            if (songSelectionPanel != null)
            {
                songSelectionPanel.SetActive(false);
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

    public void SelectSong(SongData song)
    {
        selectedSong = song;
        songSelectionPanel.SetActive(false);
        
        // Reset BeatScroller position
        GameManager.instance.theBS.transform.position = Vector3.zero;
        
        GameManager.instance.StartNewSong(song);
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