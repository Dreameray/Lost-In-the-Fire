using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;
    private float scrollSpeed;

    void Start()
    {
        RecalculateSpeed();
    }

    public void RecalculateSpeed()
    {
        // Calculate scroll speed based on BPM
        // Lower BPM = slower speed, Higher BPM = faster speed
        scrollSpeed = (beatTempo / 120f); // Normalize around 120 BPM as baseline
    }

    void Update()
    {
        if (!hasStarted) return;
        
        // Calculate movement based on BPM
        float moveSpeed = scrollSpeed * 2f; // Base movement multiplier
        transform.position -= new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        hasStarted = false;
        RecalculateSpeed();
    }
}
