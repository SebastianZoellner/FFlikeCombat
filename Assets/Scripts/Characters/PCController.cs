using System;
using UnityEngine;

public class PCController : MonoBehaviour
{
    public event Action OnActionStarted = delegate { };
    public event Action OnActionEnded = delegate { };
    public event Action OnHasActed = delegate { };


    [SerializeField] private CharacterHealth target;

    private CharacterCombat combat;
    public CharacterStats stats { get; private set; }
    private SelectionIndicator selectionIndicator;// { get; private set; }

    private bool hasActed;

    //---------------------------------------------
    //      Lifecycle Functions
    //-----------------------------------------------

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
        selectionIndicator = GetComponent<SelectionIndicator>();
    }

    private void OnEnable()
    {
        combat.OnAttackFinished += Combat_OnAttackFinished;
    }


    private void OnDisable()
    {
        combat.OnAttackFinished -= Combat_OnAttackFinished;
    }
    //---------------------------------------------
    //      Basic Getters
    //-----------------------------------------------

    public string GetName() => stats.GetName();

    //---------------------------------------------
    //      Public Methods
    //----------------------------------------------

    public void SetSelected()
    {
        if (target)
            target.selectionIndicator.SetSelected();

        selectionIndicator.SetSelected();
    }

    public void SetDeselected()
    {
        if (target)
            target.selectionIndicator.SetDeselected();

        selectionIndicator.SetDeselected();
    }

    public void StartTurn(int turnNumber)
    {
        hasActed = false;
    }

    public void StartAttack(int powerId)
    {
        if (hasActed)
        {
            Debug.Log(stats.GetName() + " has acted");
            return;
        }
        if (!stats.HasPowerID(powerId - 1))
            return;

        OnActionStarted.Invoke();

        combat.Attack(stats.GetPower(powerId - 1), target);
    }


    public void SetTarget(CharacterHealth targetHealth)
    {
        if (target)
            target.selectionIndicator.SetDeselected();
        target = targetHealth;
        if(target)
        target.selectionIndicator.SetSelected();
        //Debug.Log("Set new target: " + target.name);
    }



    //---------------------------------------------
    //      Private Methods
    //-----------------------------------------------
    private void Combat_OnAttackFinished(bool success)
    {
        OnActionEnded.Invoke();
        hasActed = success;
        if (success)
            OnHasActed.Invoke();
    }
}
