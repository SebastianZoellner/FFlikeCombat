using UnityEngine;

public class ObjectAudio : Audio
{
    [SerializeField] SimpleAudioEventSO destructionSound;

    ObjectHealth health;

    private void Awake()
    {
        health = GetComponent<ObjectHealth>();
    }

    private void OnEnable()
    {
        if (health)
            health.OnObjectDestroyed += Health_OnObjectDestroyed;
    }

    private void OnDisable()
    {
        if (health)
            health.OnObjectDestroyed -= Health_OnObjectDestroyed;
    }


    private void Health_OnObjectDestroyed()
    {
        Debug.Log("Destruction sound");
        if (destructionSound)
        {
            Debug.Log("Sounding");
            destructionSound.Play(hitAudioSource);
        }
    }
}
