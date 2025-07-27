using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject[] notePrefabs;     // Size: 4 (assign in Inspector)
    public Transform[] spawnPoints;      // Size: 4 (assign in Inspector)
    public Transform noteHolder;         // Assign in Inspector

    public float spawnInterval = 0.5f;
    private float timer = 0f;
    private bool spawning = false;

    void Update()
    {
        // Start spawning when the game starts (customize as you need)
        if (!spawning && GameManager.instance.startPlaying)
        {
            spawning = true;
        }

        if (!spawning) return;

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
        Instantiate(prefab, pos, prefab.transform.rotation, noteHolder); // Parent under NoteHolder
    }
}
