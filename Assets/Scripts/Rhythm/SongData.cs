using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Rhythm Game/Song Data")]
public class SongData : ScriptableObject
{
    public string songName;
    public AudioClip audioClip;
    public float bpm;
    public float noteSpawnInterval;
    public Sprite albumCover;
    public string difficulty;
}