using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] AudioSource attackAudioSource;
    [SerializeField] AudioSource hitAudioSource;

    public void SetHitSound(PowerSO attackPower, CharacterHealth target)
    {
        target.GetComponent<CharacterAudio>().PlayHitSound(attackPower);
    }

    public void PlayHitSound(PowerSO attackPower)
    {
        attackPower.PlayHitSound(hitAudioSource);
    }

    public void PlayAttackSound(PowerSO attackPower)
    {
        attackPower.PlayAttackSound(attackAudioSource);
    }

    public void PlayShootSound(PowerSO attackPower)
    {
        attackPower.PlayHitSound(attackAudioSource);
    }

    public void PlayMissileHitSound(SimpleAudioEventSO impactSFX)
    {
        impactSFX.Play(hitAudioSource);
    }
}
