using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public event Action OnAttackAnimationFinished = delegate { };

    [SerializeField] bool isAnimated = false;
    private Animator animator;

    private readonly int moveAnimationHash = Animator.StringToHash("Move");
    private readonly int attackAnimationHash = Animator.StringToHash("Attack");
    private readonly int downAnimationHash = Animator.StringToHash("Down");
    private readonly int diedAnimationHash = Animator.StringToHash("Died");
    private readonly int hitAnimationHash = Animator.StringToHash("Hit");
    private readonly int moveStateNameHash = Animator.StringToHash("Base Layer.Moving");

    private AnimatorOverrideController overrideController;
    private bool isAttacking;
    private float checkAttackingtimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)
            Debug.LogError(name+" No animator component found");
        overrideController = GetComponent<CharacterStats>().GetAnimatorOverrideController();
    }

    private void Update()
    {
        if (!isAttacking)
            return;

        checkAttackingtimer += Time.deltaTime;
        if (checkAttackingtimer < 0.3)
            return;

        AnimatorStateInfo info= animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log("Checking Attack "+checkAttackingtimer);
        if(!info.IsName("Base Layer.Attacking"))
            {
            //Debug.Log("AttackFinished");
            isAttacking = false;
            OnAttackAnimationFinished.Invoke();
        }
    }

    public void SetMove(bool move)
    {
        //Debug.Log("Move animation: " + move);
        animator.SetBool(moveAnimationHash, move);
    }

    public void SetAttack(AnimationClip attackAnimation)
    {
        if (isAttacking)
            return;

        if (!isAnimated)
        {
            OnAttackAnimationFinished.Invoke();
            return;
        }
        //Debug.Log("Starting attack animation");
        if (attackAnimation)
        {
            overrideController["Attack"] = attackAnimation;
            animator.runtimeAnimatorController = overrideController;
        }
        animator.SetTrigger(attackAnimationHash);
        isAttacking = true;
        checkAttackingtimer = 0;
    }

    public void SetHit()
    {
        if (!isAnimated)
            return;

        animator.SetTrigger(hitAnimationHash);
    }

    public void SetDown()
    {
        if (!isAnimated)
            return;

        animator.SetTrigger(downAnimationHash);
    } 

    public void SetDied()
    {
        if (!isAnimated)
            return;

        animator.SetTrigger(diedAnimationHash);
    }
}
