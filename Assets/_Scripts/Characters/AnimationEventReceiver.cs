using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    CharacterCombat combat;
    CharacterAudio characterAudio;
    CharacterVFX characterVFX;

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        characterAudio = GetComponent<CharacterAudio>();
        characterVFX = GetComponent<CharacterVFX>();
    }
    private void Step()
    {
        characterAudio.PlayStep();
    }

    private void Drop()
    {
        characterAudio.PlayDrop();
    }

    private void AttackSound() //This is an animation event
    {
        characterAudio.PlayAttackSound();
    }

    private void Buff()
    {
        combat.StartBuff();
    }

    private void Impact()
    {
        combat.ManageImpact();
    }

    private void Projectile()
    {
        combat.LaunchProjectile();
    }

    private void StartTrail()
    {
        characterVFX.EnableTrail();
    }

    private void StopTrail()
    {
        characterVFX.DisableTrail();
    }
}
