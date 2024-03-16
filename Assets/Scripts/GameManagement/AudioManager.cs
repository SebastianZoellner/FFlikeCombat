using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambienceSource;

    private Coroutine musicCoroutine;

    private int currentIndex = 0;

    public void PlayAmbiance(AudioClip ambience)
    {
       
        ambienceSource.loop = true;
        ambienceSource.clip = ambience;
        ambienceSource.Play();
    }

    public void PlayMusic(AudioClip[] musicArray)
    {
        musicCoroutine=StartCoroutine(PlaySequentialClips(musicArray));
    }
    public void StopAll()
    {
        ambienceSource.Stop();
        StopCoroutine(musicCoroutine);
        musicSource.Stop();
    }

    IEnumerator PlaySequentialClips(AudioClip[] audioClips)
    {
        // Loop indefinitely
        while (true)
        {
            // Set the current audio clip to play
            musicSource.clip = audioClips[currentIndex];

            // Play the current audio clip
            musicSource.Play();

            // Wait until the current clip finishes playing
            yield return new WaitForSeconds(audioClips[currentIndex].length);

            // Increment the index for the next clip
            currentIndex = (currentIndex + 1) % audioClips.Length;
        }
    }
}
