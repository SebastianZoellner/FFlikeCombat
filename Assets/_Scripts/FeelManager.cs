using UnityEngine;
using MoreMountains.Feedbacks;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;

public class FeelManager : MonoBehaviour
{
    [SerializeField] MMFeedbacks startAttackFeedback;
    [SerializeField] MMFeedbacks resetFeedbacks;
    [SerializeField] MMFeedbacks hitFeedback;
    [SerializeField] MMFeedbacks startAllAttackFeedback;
    [SerializeField] MMF_Player levelUpFeedback;

   [SerializeField] ActionCameraController actionCamera;
    [SerializeField] CinemachineVirtualCamera lookAtCamera;

    [SerializeField] CharacterExperience testCharacter;
    
    private void OnEnable()
    {
        CharacterCombat.OnAnyAttackStarted += CharacterCombat_OnAnyAttackStarted;
        CharacterCombat.OnAnyAttackEnded += EndAttack;
        CharacterCombat.OnAnyPowerHit += HitEffect;
        CharacterExperience.OnAnyLevelUp += LevelUp;
    }

    private void OnDisable()
    {
        CharacterCombat.OnAnyAttackStarted -= CharacterCombat_OnAnyAttackStarted;
        CharacterCombat.OnAnyAttackEnded -= EndAttack;
        CharacterCombat.OnAnyPowerHit -= HitEffect;
        CharacterExperience.OnAnyLevelUp -= LevelUp;
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

    [Button]
    private void TestLevelUp()
    {
        LevelUp(testCharacter);
    }

    private void LevelUp(CharacterExperience experience)
    {
        MMF_ParticlesInstantiation particleEffect = levelUpFeedback.GetFeedbackOfType<MMF_ParticlesInstantiation>();
        particleEffect.InstantiateParticlesPosition = experience.transform;

        MMF_Animation animationStart = levelUpFeedback.GetFeedbackOfType<MMF_Animation>();
        animationStart.BoundAnimator = experience.GetComponent<Animator>();

        lookAtCamera.LookAt = experience.transform;
        lookAtCamera.Follow = experience.transform;

        levelUpFeedback.PlayFeedbacks();
    }

    
}
