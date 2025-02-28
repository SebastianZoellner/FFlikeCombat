using UnityEngine;

public class CharacterAudio : Audio
{
    [SerializeField] AudioSource attackAudioSource;  
    [SerializeField] AudioSource stepAudioSource;  
    [SerializeField] SimpleAudioEventSO step;
    [SerializeField] SimpleAudioEventSO dropSound;
    [SerializeField] SimpleAudioEventSO levelSound;

    private PowerSO attack;
    private CharacterCombat characterCombat;
    private CharacterExperience experience;

    private void Awake()
    {
        characterCombat = GetComponent<CharacterCombat>();
        experience = GetComponent<CharacterExperience>();
    }

    private void OnEnable()
    {if(experience)
        GetComponent<CharacterExperience>().OnLevelUp += CharacterExperience_OnLevelUp;
    }
    private void OnDisable()
    {if(experience)
        GetComponent<CharacterExperience>().OnLevelUp -= CharacterExperience_OnLevelUp;
    }

   

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
    public void PlayStep()
    {
        if(step)
        step.Play(stepAudioSource);
    }

   public void PlayDrop()
    {
        if(dropSound)
        dropSound.Play(stepAudioSource);
    }

    public void PlayAttackSound() 
    {
        if (attack)
            attack.PlayAttackSound(attackAudioSource);
    }

    //------------------------------------------------------------------------
    //                  Private functions
    //------------------------------------------------------------------------


    private void CharacterExperience_OnLevelUp()
    {
        if (levelSound)
            levelSound.Play(attackAudioSource);
    }
}
