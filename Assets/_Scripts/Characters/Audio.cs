using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Audio : MonoBehaviour
{
    [SerializeField] protected AudioSource hitAudioSource;

    public void PlayHitSound(SimpleAudioEventSO hitSFX)
    {
        if (!hitSFX)
            return;
        hitSFX.Play(hitAudioSource);
    }
}
