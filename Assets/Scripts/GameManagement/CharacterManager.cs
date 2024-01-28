using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public PCController[] playerCharacterArray;
    public EnemyController[] enemyArray;

    [SerializeField] InputReader input;

    private PCController activeCharacter;

    private bool actionOngoing;

    private void OnEnable()
    {
        input.OnAttackSelected += Input_OnAttackSelected;
        input.OnTargetSelected += Input_OnTargetSelected;
    }

    private void Start()
    {
        SelectNewCharacter( playerCharacterArray[0]);
    }

    private void OnDisable()
    {
        input.OnAttackSelected -= Input_OnAttackSelected;
        input.OnTargetSelected -= Input_OnTargetSelected;
    }

    private void Input_OnAttackSelected(int attackId)
    {
        if (actionOngoing)
            return;
        Debug.Log("Starting attack " + attackId);
        activeCharacter.StartAttack(attackId);
    }

   

    private void Input_OnTargetSelected(CharacterHealth characterHealth)
    {
        if (actionOngoing)
            return;

        PCController selectedCharacter = characterHealth.GetComponent<PCController>();
        if (selectedCharacter)
            SelectNewCharacter(selectedCharacter);
        else
            activeCharacter.SetTarget(characterHealth);
    }

    private void SelectNewCharacter(PCController newCharacter)
    {
        if (activeCharacter)
        {
            activeCharacter.OnActionStarted -= ActiveCharacter_OnActionStarted;
            activeCharacter.OnActionEnded -= ActiveCharacter_OnActionEnded;
        }
        Debug.Log("New Character Selected " + newCharacter.stats.GetName());
        activeCharacter = newCharacter;
        activeCharacter.OnActionStarted += ActiveCharacter_OnActionStarted;
        activeCharacter.OnActionEnded += ActiveCharacter_OnActionEnded;
            }

    private void ActiveCharacter_OnActionEnded()
    {
        actionOngoing = false;
    }

    private void ActiveCharacter_OnActionStarted()
    {
        actionOngoing = true;
    }
}
