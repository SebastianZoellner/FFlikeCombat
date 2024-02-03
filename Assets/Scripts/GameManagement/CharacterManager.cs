using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static event Action<CharacterHealth> OnPlayerSelectedChanged = delegate { };
    public static event Action<CharacterHealth> OnEnemySelectedChanged = delegate { };

    public List<PCController> playerCharacterList;
    public List<EnemyController> enemyList;

    [SerializeField] InputReader input;

    private PCController activeCharacter;

    private bool actionOngoing;


    //-----------------------------------------
    //      Lifecycle Methods
    //-----------------------------------------

    private void OnEnable()
    {
        input.OnCharacterSelected += Input_OnCharacterSelected;
        input.OnAttackSelected += Input_OnAttackSelected;
        input.OnDeselected += Input_OnDeselected;
        CharacterHealth.OnAnyPCDied += CharacterHealth_OnAnyPCDied;
        CharacterHealth.OnAnyEnemyDied+= CharacterHealth_OnAnyEnemyDied;
    }

    private void Start()
    {
        SelectNewCharacter(playerCharacterList[0]);
    }

    private void OnDisable()
    {
        input.OnCharacterSelected -= Input_OnCharacterSelected;
        input.OnAttackSelected -= Input_OnAttackSelected;      
        input.OnDeselected += Input_OnDeselected;
        CharacterHealth.OnAnyPCDied -= CharacterHealth_OnAnyPCDied;
        CharacterHealth.OnAnyEnemyDied -= CharacterHealth_OnAnyEnemyDied;
    } 
    //-----------------------------------------
    //      Public Methods
    //-----------------------------------------

    public bool PressAttackButton(int attackID)
    {
        if (actionOngoing)
            return false;
        //Debug.Log("Starting attack " + attackId);
        activeCharacter.StartAttack(attackID);
        return true;
    }

    //-----------------------------------------
    //      Private Methods
    //-----------------------------------------

    private void Input_OnAttackSelected(int attackId)
    {
        if (actionOngoing)
            return;
        //Debug.Log("Starting attack " + attackId);
        activeCharacter.StartAttack(attackId);
    }

    private void Input_OnCharacterSelected(CharacterHealth health)
    {
        if (!health)
        {
            Input_OnDeselected();
            return;
        }

        PCController selectedPlayer = health.GetComponent<PCController>();

        if(!selectedPlayer)//enemy was selected
        {
            SelectEnemy(health);
            return;
        }

        SelectNewCharacter(selectedPlayer);
    }

    private void SelectEnemy(CharacterHealth health)
    {
        if (activeCharacter)
        {
            activeCharacter.SetTarget(health);
            OnEnemySelectedChanged.Invoke(health);
        }

        return;
    }


    private void Input_OnDeselected()
    {
        if (!activeCharacter) return;

        activeCharacter.SetTarget(null);
        activeCharacter.SetDeselected();

        OnPlayerSelectedChanged.Invoke(null);
        OnEnemySelectedChanged.Invoke(null);

        activeCharacter = null;
    }

    private void SelectNewCharacter(PCController newCharacter)
    {
        if (activeCharacter)
        {
            activeCharacter.OnActionStarted -= ActiveCharacter_OnActionStarted;
            activeCharacter.OnActionEnded -= ActiveCharacter_OnActionEnded;
            activeCharacter.SetDeselected();
        }
        Debug.Log("New Character Selected " + newCharacter.stats.GetName());
        activeCharacter = newCharacter;
        activeCharacter.OnActionStarted += ActiveCharacter_OnActionStarted;
        activeCharacter.OnActionEnded += ActiveCharacter_OnActionEnded;
        activeCharacter.SetSelected();
        OnPlayerSelectedChanged.Invoke(newCharacter.GetComponent<CharacterHealth>());
    }

    private void ActiveCharacter_OnActionEnded()
    {
        actionOngoing = false;
    }

    private void ActiveCharacter_OnActionStarted()
    {
        actionOngoing = true;
    }

    private void CharacterHealth_OnAnyPCDied(CharacterHealth deadPCHealth)
    {
        PCController deadPC = deadPCHealth.GetComponent<PCController>();
        playerCharacterList.Remove(deadPC);
    }

    private void CharacterHealth_OnAnyEnemyDied(CharacterHealth deadEnemyHealth)
    {
        EnemyController deadEnemy = deadEnemyHealth.GetComponent<EnemyController>();
        enemyList.Remove(deadEnemy);
    }
}
