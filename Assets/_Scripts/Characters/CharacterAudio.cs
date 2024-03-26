using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] AudioSource attackAudioSource;
    [SerializeField] AudioSource hitAudioSource;
    [SerializeField] AudioSource stepAudioSource;
    
    [SerializeField] SimpleAudioEventSO step;
    [SerializeField] SimpleAudioEventSO dropSound;

    public void SetHitSound(PowerSO attackPower, CharacterHealth target)
    {
        target.GetComponent<CharacterAudio>().PlayHitSound(attackPower.hitSound);
    }

    

    public void PlayAttackSound(PowerSO attackPower)
    {
        attackPower.PlayAttackSound(attackAudioSource);
    }

    public void PlayShootSound(SimpleAudioEventSO shootSFX)
    {
        if (!shootSFX)
            return;

        shootSFX.Play(attackAudioSource);
    }

    public void PlayHitSound(SimpleAudioEventSO hitSFX)
    {
        if (!hitSFX)
            return;
        hitSFX.Play(hitAudioSource);
    }

    private void Step()
    {
        if(step)
        step.Play(stepAudioSource);
    }

   private void Drop()
    {
        if(dropSound)
        dropSound.Play(stepAudioSource);
    }        
}
