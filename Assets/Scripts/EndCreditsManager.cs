using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndCreditsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private GameObject loadingPanel; // Add this field

    private void Start()
    {
        StartCoroutine(PrepareVideo());
    }

    private IEnumerator PrepareVideo()
    {
        // Show loading panel
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // Ensure menu is hidden
        menuCanvas.SetActive(false);

        // Disable Play On Awake temporarily
        bool wasPlayOnAwake = videoPlayer.playOnAwake;
        videoPlayer.playOnAwake = false;

        // Add video completion listener
        videoPlayer.loopPointReached += OnVideoComplete;

        // Find LevelLoader if not assigned
        if (levelLoader == null)
            levelLoader = FindObjectOfType<LevelLoader>();

        // Stop and prepare video
        videoPlayer.Stop();
        videoPlayer.time = 0;
        videoPlayer.Prepare();
        
        // Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Add small delay for buffer
        yield return new WaitForSeconds(1f);

        // Hide loading panel
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
        
        // Wait two frames to ensure UI and video are ready
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        // Restore Play On Awake setting
        videoPlayer.playOnAwake = wasPlayOnAwake;
        
        // Start video from beginning
        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    private void OnVideoComplete(VideoPlayer vp)
    {
        menuCanvas.SetActive(true);
    }

    // Button click handlers
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void RestartFromBeginning()
    {
        if (levelLoader != null)
        {
            // Reset replay flag when starting from beginning
            SongSelectionManager.isReplay = false;
            SceneManager.LoadScene(0);
        }
    }

    public void PlayRhythmGame()
    {
        if (levelLoader != null)
        {
            // Set replay flag before loading scene
            SongSelectionManager.isReplay = true;
            SceneManager.LoadScene(2); // RhythmScene
        }
    }
}