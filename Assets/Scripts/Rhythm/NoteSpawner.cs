using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject[] notePrefabs;     // Size: 4 (assign in Inspector)
    public Transform[] spawnPoints;      // Size: 4 (assign in Inspector)
    public Transform noteHolder;         // Assign in Inspector

    public float spawnInterval = 0.5f;
    private float timer = 0f;
    private bool spawning = false;
    
    private float songTime => GameManager.instance.theMusic.time;


    public void ResetSpawner()
    {
        spawning = false;
        timer = 0f;

        // Clear any existing notes
        foreach (Transform child in noteHolder)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        if (!GameManager.instance.startPlaying)
            return;

        var song = GameManager.selectedSong;
        if (song == null)
            return;

        // Start spawning only after song.noteSpawnStartDelay
        if (!spawning && songTime >= song.noteSpawnStartDelay)
        {
            spawning = true;
            timer = 0f;
        }

        // Stop spawning before end, using noteSpawnEndOffset
        float endOffset = song.noteSpawnEndOffset;
        if (GameManager.instance.theMusic.clip != null &&
            songTime >= GameManager.instance.theMusic.clip.length - endOffset)
        {
            spawning = false;
            return;
        }

        if (!spawning)
            return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnNote(Random.Range(0, 4));
        }
    }


    void SpawnNote(int direction)
    {
        var prefab = notePrefabs[direction];
        var pos = spawnPoints[direction].position;
        Instantiate(prefab, pos, prefab.transform.rotation, noteHolder);
        GameManager.instance.totalNotes++; // Add this line
    }
}
