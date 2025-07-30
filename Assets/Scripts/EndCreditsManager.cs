using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndCreditsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private LevelLoader levelLoader;

    private void Start()
    {
        // Ensure menu is hidden at start
        menuCanvas.SetActive(false);

        // Add video completion listener
        videoPlayer.loopPointReached += OnVideoComplete;

        // Find LevelLoader if not assigned
        if (levelLoader == null)
        {
            levelLoader = FindObjectOfType<LevelLoader>();
        }
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
        // Load first scene (index 0)
        if (levelLoader != null)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void PlayRhythmGame()
    {
        // Load rhythm game scene directly
        if (levelLoader != null)
        {
            // Assuming RhythmScene is at index 2
            SceneManager.LoadScene(2);
        }
    }
}