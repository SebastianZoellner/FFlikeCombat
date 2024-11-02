using UnityEngine;
using MoreMountains.Feedbacks;

public class FeelManager : MonoBehaviour
{
    [SerializeField] MMFeedbacks startAttackFeedback;
    [SerializeField] MMFeedbacks resetFeedbacks;
    [SerializeField] MMFeedbacks hitFeedback;
    [SerializeField] MMFeedbacks startAllAttackFeedback;

   [SerializeField] ActionCameraController actionCamera;


    private void OnEnable()
    {
        CharacterCombat.OnAnyAttackStarted += CharacterCombat_OnAnyAttackStarted;
        CharacterCombat.OnAnyAttackEnded += EndAttack;
        CharacterCombat.OnAnyPowerHit += HitEffect;
    }

    private void OnDisable()
    {
        CharacterCombat.OnAnyAttackStarted -= CharacterCombat_OnAnyAttackStarted;
        CharacterCombat.OnAnyAttackEnded -= EndAttack;
        CharacterCombat.OnAnyPowerHit -= HitEffect;
    }

    private void CharacterCombat_OnAnyAttackStarted(CharacterHealth attacker, CharacterHealth target, PowerSO power)
    {
        if (!attacker.GetComponent<PCController>())
            return;

        switch (power.target)
        {
            case TargetType.Enemy:
                StartAttack(attacker.transform, power);
                break;
            case TargetType.AllEnemies:
                StartAllAttack(attacker.transform, power);
                break;
        }
    }

    private void StartAttack(Transform attackerTransform, PowerSO attackPower)
    {
        actionCamera.SetCameraFocus(attackerTransform);
        startAttackFeedback.PlayFeedbacks();
    }

    private void EndAttack()
    {
        resetFeedbacks.PlayFeedbacks();
    }

    private void HitEffect()
    {
        hitFeedback.PlayFeedbacks();
    }

    private void StartAllAttack(Transform transform, PowerSO attackPower)
    {
        startAllAttackFeedback.PlayFeedbacks();
    }
}
