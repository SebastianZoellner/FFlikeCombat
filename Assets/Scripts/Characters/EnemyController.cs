using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public event Action OnTurnFinished = delegate { };
    CharacterHealth target;
    PowerSO selectedPower;

    CharacterCombat combat;
    CharacterStats stats;
    CharacterInitiative initiative;

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
        initiative = GetComponent<CharacterInitiative>();
    }

    public void TakeTurn()
    {
        target = AIBrain.Instance.SelectWeakestTarget();
        selectedPower = AIBrain.Instance.SelectPower(stats);
        combat.StartAttack(selectedPower, target);
        OnTurnFinished.Invoke();
    }

    public void SetNextAction()
    {
        target = AIBrain.Instance.SelectWeakestTarget();
        if (!target)
            Debug.LogWarning("No target Selected");

        selectedPower = AIBrain.Instance.SelectPower(stats);
        if (!selectedPower)
            Debug.LogWarning("No power Selected");
       
        Debug.Log("Setting Attack " + selectedPower.buttonName + " against " + target.name);
        initiative.ReadyAttack(selectedPower,target);
    }
}
