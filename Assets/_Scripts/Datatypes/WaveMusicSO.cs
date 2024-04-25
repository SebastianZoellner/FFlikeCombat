using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Events/WaveMusic")]
public class WaveMusicSO : ScriptableObject
{
    public Song[] SongArray;
}

[Serializable]
public struct Song
{
    public AudioClip music;
    public float volume;
}
