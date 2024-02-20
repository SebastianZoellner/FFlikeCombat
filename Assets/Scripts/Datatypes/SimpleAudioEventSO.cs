
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName ="Audio Events/Simple")]
public class SimpleAudioEventSO : ScriptableObject
{
    [SerializeField] private AudioClip[] clips = new AudioClip[0];
    [MinMaxSlider(0,2,true)]
    [SerializeField] private Vector2 volumeRange = new Vector2(0.9f, 1.1f);

    [MinMaxSlider(0, 2, true)]
    [SerializeField]     private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [MinMaxSlider(0, 1000, true)]
    [SerializeField]  private Vector2 distance = new Vector2(1, 1000);
    [SerializeField] private AudioMixerGroup mixer;
    [Range(0f,1f)]
    [SerializeField] private float spatialBlend=1;

    public void Play(AudioSource source)
    {
        source.outputAudioMixerGroup = mixer;

        int clipIndex = UnityEngine.Random.Range(0, clips.Length);
        source.clip = clips[clipIndex];
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.volume = Random.Range(volumeRange.x, volumeRange.y);
        source.spatialBlend = spatialBlend;
        source.minDistance = distance.x;
        source.maxDistance = distance.y;

        source.Play();
    }
}
