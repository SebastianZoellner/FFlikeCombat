using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAvatarControl : MonoBehaviour
{
    [SerializeField] private bool isPlayerAvatar;
    SetAvatarDisplay setDisplay;


    private void Awake()
    {
        setDisplay = GetComponent<SetAvatarDisplay>();
    }

    private void OnEnable()
    {
        CharacterManager.OnPlayerSelectedChanged += InputReader_OnPlayerSelectedChanged;
        ActionSequencer.OnNoActor += ActionSequencer_OnNoActor;
        CharacterCombat.OnAnyAttackStarted += CharacterCombat_OnAnyAttackStarted;
        CharacterManager.OnEnemySelectedChanged += InputReader_OnEnemySelectedChanged;
    }

    private void OnDisable()
    {
        CharacterManager.OnPlayerSelectedChanged -= InputReader_OnPlayerSelectedChanged;
        ActionSequencer.OnNoActor -= ActionSequencer_OnNoActor;
        CharacterCombat.OnAnyAttackStarted -= CharacterCombat_OnAnyAttackStarted;
        CharacterManager.OnEnemySelectedChanged -= InputReader_OnEnemySelectedChanged;
    }

    private void InputReader_OnPlayerSelectedChanged(CharacterHealth health)
    {
        if (!isPlayerAvatar)
            return;

        setDisplay.SetAvatar(health);
    }

    private void ActionSequencer_OnNoActor()
    {
        setDisplay.HideAvatar();
    }

    private void CharacterCombat_OnAnyAttackStarted(CharacterHealth attacker, CharacterHealth defender, PowerSO power)
    {
        if (isPlayerAvatar)
        {
            if (attacker && attacker.GetComponent<PCController>())
                setDisplay.SetAvatar(attacker);

            if (defender && defender.GetComponent<PCController>())
                setDisplay.SetAvatar(defender);

            return;
        }
        else
        {
            if (attacker && attacker.GetComponent<EnemyController>())
                setDisplay.SetAvatar(attacker);

            if (defender && defender.GetComponent<EnemyController>())
                setDisplay.SetAvatar(defender);
        }
    }

    private void InputReader_OnEnemySelectedChanged(IDamageable health)
    {
        if (isPlayerAvatar)
            return;
        
        if (health is CharacterHealth)
            setDisplay.SetAvatar((CharacterHealth)health);
    }
}
