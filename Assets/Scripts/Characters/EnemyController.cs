using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public event Action OnTurnFinished = delegate { };
    CharacterHealth target;
    PowerSO selectedPower;

    CharacterCombat combat;
    CharacterStats stats;

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
    }

    public void TakeTurn()
    {
        target = AIBrain.Instance.SelectWeakestTarget();
        selectedPower = AIBrain.Instance.SelectPower(stats);
        combat.Attack(selectedPower, target);
        OnTurnFinished.Invoke();
    }
}
