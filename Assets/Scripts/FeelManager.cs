using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class FeelManager : MonoBehaviour
{
    public static FeelManager Instance;

    [SerializeField] MMFeedbacks startAttackFeedback;
    [SerializeField] MMFeedbacks resetFeedbacks;
    [SerializeField] MMFeedbacks hitFeedback;

   [SerializeField] ActionCameraController actionCamera;


    private void Awake()
    {
        Instance = this;
    }

    public void StartAttack(Transform attackerTransform, PowerSO attackPower)
    {
        actionCamera.SetCameraFocus(attackerTransform);
        startAttackFeedback.PlayFeedbacks();
    }
    public void EndAttack()
    {
        resetFeedbacks.PlayFeedbacks();
    }
    public void HitEffect()
    {
        hitFeedback.PlayFeedbacks();
    }
}
