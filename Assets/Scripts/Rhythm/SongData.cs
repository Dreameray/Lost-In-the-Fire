using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Song", menuName = "Rhythm Game/Song Data")]
public class SongData : ScriptableObject
{
    public string songName;
    public AudioClip audioClip;
    public float bpm;
    public float noteSpawnInterval;
    public Sprite albumCover;
    public string difficulty;

    [Header("Optional Visuals")]
    public VideoClip videoClip; // New field for music video

    [Header("Timing Offsets")]
    public float noteSpawnStartDelay = 0f;   // Time to delay the start of note spawn
    public float noteSpawnEndOffset = 2f;    // Time after music ends to stop spawning

}