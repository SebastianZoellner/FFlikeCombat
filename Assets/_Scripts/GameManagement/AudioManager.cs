using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioSource UIFXSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambienceSource;
    [SerializeField] AudioMixer audioMixer;

    private Coroutine musicCoroutine;

    private int currentIndex = 0;

    private void Awake()
    {
        UIFXSource = gameObject.AddComponent<AudioSource>();
    }

   
    public void PlayAmbiance(AudioClip ambience)
    {
       
        ambienceSource.loop = true;
        ambienceSource.clip = ambience;
        ambienceSource.Play();
    }

    public void PlayMusic(WaveMusicSO music)
    {
        musicCoroutine=StartCoroutine(PlaySequentialClips(music));
    }
    public void StopAll()
    {
        if(musicCoroutine!=null)
            StopCoroutine(musicCoroutine);

        ambienceSource.Stop();  
        musicSource.Stop();
    }

    public void AdjustVolume(float volume, AudioMixerChannel channel) 
    {
        switch (channel)
        {
            case AudioMixerChannel.Music:
                audioMixer.SetFloat("MusicVolume", volume);
                break;
            case AudioMixerChannel.Background:
                audioMixer.SetFloat("BackgroundVolume", volume);
                break;
            case AudioMixerChannel.SFX:
                audioMixer.SetFloat("SFXVolume", volume);
                break;
        }    
    }


    IEnumerator PlaySequentialClips(WaveMusicSO musicSO)
    {
        //Debug.Log("Starting Music PlaySequentialClips");
        currentIndex = 0;
        // Loop indefinitely
        while (true)
        {
            // Set the current audio clip to play
            musicSource.clip = musicSO.SongArray[currentIndex].music;
            musicSource.volume= musicSO.SongArray[currentIndex].volume;
            //Debug.Log("Playing Clip " + currentIndex);

            // Play the current audio clip
            musicSource.Play();

            // Wait until the current clip finishes playing
            yield return new WaitForSeconds(musicSO.SongArray[currentIndex].music.length);

            // Increment the index for the next clip
            currentIndex = (currentIndex + 1) % musicSO.SongArray.Length;
        }
    }

    public enum AudioMixerChannel
    {
        Music,
        Background,
        SFX
    }
}
