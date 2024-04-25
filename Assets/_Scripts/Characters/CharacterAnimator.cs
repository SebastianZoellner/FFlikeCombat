using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public event Action OnActionAnimationFinished = delegate { };

    public bool Initialized { get; private set; } = false;

    [SerializeField] bool isAnimated = true;
    [SerializeField] bool isFlying = false;
    private Animator animator;

    private readonly int moveAnimationHash = Animator.StringToHash("Move");
    private readonly int attackAnimationHash = Animator.StringToHash("Attack");
    private readonly int downAnimationHash = Animator.StringToHash("Down");
    private readonly int diedAnimationHash = Animator.StringToHash("Died");
    private readonly int hitAnimationHash = Animator.StringToHash("Hit");
    private readonly int buffAnimationHash = Animator.StringToHash("Buff");
    private readonly int moveStateNameHash = Animator.StringToHash("Base Layer.Moving");
    private readonly int idleTriggerHash = Animator.StringToHash("Idle");
    private readonly int raisedTriggerHash=Animator.StringToHash("Raised");

    private AnimatorOverrideController overrideController;
    private bool isAttacking;
    private bool isBuffing;
    private float checkAttackingtimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)             Debug.LogError(name+" No animator component found");
        overrideController = GetComponent<CharacterStats>().GetAnimatorOverrideController();

        Initialized = true;
    }

    private void Update()
    {
        if (isBuffing)
        {
            CheckBuffingAnimation();
            return;
        }

        if (!isAttacking)
            return;

        checkAttackingtimer += Time.deltaTime;
        if (checkAttackingtimer < 0.3)
            return;

        AnimatorStateInfo info= animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log("Checking Attack "+checkAttackingtimer);

        if(!info.IsName("Base.Attacking"))
            {
            //Debug.Log("Attack Finished");
            isAttacking = false;
            OnActionAnimationFinished.Invoke();
        }
    }

    

    public void SetMove(bool move)
    {
        animator.ResetTrigger(hitAnimationHash);
        animator.SetBool(moveAnimationHash, move);
    }

    public void SetAttack(PowerSO attack)
    {
        if (isAttacking)
            return;

        if (!isAnimated)
        {
            OnActionAnimationFinished.Invoke();
            return;
        }
        //Debug.Log("Starting attack animation");
        if (attack.attackAnimation)
        {
            if (isFlying && attack.flyingAnimation)
                overrideController["Attack"] = attack.flyingAnimation;
            else
            overrideController["Attack"] = attack.attackAnimation;
            animator.runtimeAnimatorController = overrideController;
        }
        animator.SetTrigger(attackAnimationHash);
        isAttacking = true;
        checkAttackingtimer = 0;
    }

   public void SetBuff()
    {
        if (!isAnimated)
            return;

        animator.SetTrigger(buffAnimationHash);
        isBuffing = true;
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


    public void SetRaised()
    {
        if (!isAnimated)
            return;

        animator.SetTrigger(raisedTriggerHash);
    }

    public void SetIdle()
    {
        {
            if (!isAnimated)
                return;

            animator.SetTrigger(idleTriggerHash);
        }

    }

    private void CheckBuffingAnimation()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Base.Buffing"))
            Debug.Log("In Base.Buffing " + info.normalizedTime);

        if (info.IsName("Base.Buffing") && info.normalizedTime >= 0.95f)
        {
            isBuffing = false;
            OnActionAnimationFinished.Invoke();
        }


    }
}
