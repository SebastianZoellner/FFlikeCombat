using System;
using UnityEngine;

public class PCController : MonoBehaviour
{
    public event Action OnActionStarted = delegate { };
    public event Action OnActionEnded = delegate { };
    public event Action OnHasActed = delegate { };


    [SerializeField] private CharacterHealth target;

    //private CharacterCombat combat;
    private CharacterInitiative initiative;
    public CharacterStats stats { get; private set; }
    private SelectionIndicator selectionIndicator;// { get; private set; }

    //private bool hasActed;

    //---------------------------------------------
    //      Lifecycle Functions
    //-----------------------------------------------

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        initiative = GetComponent<CharacterInitiative>();
        selectionIndicator = GetComponent<SelectionIndicator>();
    }

    private void OnEnable()
    {
        CharacterHealth.OnAnyEnemyDied += OnAnyEnemyDied;
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

    

    public void StartAttack(PowerSO power)
    {
        if (!target)
            return;

        

        initiative.ReadyAttack(power, target);
    }


    public void SetTarget(CharacterHealth targetHealth)
    {
        if (!targetHealth.canBeTarget)
            return;

        if (target)
            target.selectionIndicator.SetDeselected();

        target = targetHealth;
        if(target)
        target.selectionIndicator.SetSelected();
        //Debug.Log("Set new target: " + target.name);
    }

    //---------------------------------------------
    //      Private Methods
    //---------------------------------------------

    private void OnAnyEnemyDied(CharacterHealth deadHealth)
    {
        if (deadHealth == target)
        {
            target = null;
            deadHealth.selectionIndicator.SetDeselected();
        }
    }

}
