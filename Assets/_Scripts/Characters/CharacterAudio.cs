using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : Audio
{
    [SerializeField] AudioSource attackAudioSource;  
    [SerializeField] AudioSource stepAudioSource;  
    [SerializeField] SimpleAudioEventSO step;
    [SerializeField] SimpleAudioEventSO dropSound;

    private PowerSO attack;

    public void SetHitSound(PowerSO attackPower, Transform target)
    {
        Audio audio = target.GetComponent<Audio>();
        if (!audio)
            Debug.Log("No Audio component found on " + target.name);
        target.GetComponent<Audio>().PlayHitSound(attackPower.hitSound);
    }



    public void SetAttack(PowerSO attackPower)
    {
        attack = attackPower;
    }

   

    public void PlayShootSound(SimpleAudioEventSO shootSFX)
    {
        if (!shootSFX)
            return;

        shootSFX.Play(attackAudioSource);
    }

   
    //Animation Events
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

    private void PlayAttackSound() //This is an animation event
    {
        if (attack)
            attack.PlayAttackSound(attackAudioSource);
    }
}
