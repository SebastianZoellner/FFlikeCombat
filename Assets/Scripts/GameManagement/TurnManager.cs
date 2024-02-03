using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int turnNumber;
    private int charactersActed;

    [SerializeField] CharacterManager characterManager;

    private EnemyController activeEnemy;
    private int activeEnemyId;
    


    private void OnEnable()
    {
        foreach (PCController pcc in characterManager.playerCharacterList)
        {
            pcc.OnHasActed += PCC_OnHasActed;
        }
    }

    private void Start()
    {
        StartPlayerTurn();
    }

    private void OnDisable()
    {
        foreach (PCController pcc in characterManager.playerCharacterList)
        {
            pcc.OnHasActed -= PCC_OnHasActed;
        }
    }

    private void StartPlayerTurn()
    {
        turnNumber++;
        Debug.Log("Starting Turn " + turnNumber);
        charactersActed = 0;
        foreach (PCController pcc in characterManager.playerCharacterList)
        {
            pcc.StartTurn(turnNumber);
            pcc.GetComponent<StatusManager>().StartTurn();
        }
    }

    private void PCC_OnHasActed()
    {
        charactersActed++;
        if (charactersActed == characterManager.playerCharacterList.Count)
            EndPlayerTurn(); ;
    }

    private void EndPlayerTurn()
    {
        Debug.Log("Player turn has ended");
        EnemyTurn();
    }

    private void EnemyTurn()
    {
        activeEnemyId = 0;
        foreach (EnemyController ec in characterManager.enemyList)
        {          
            ec.GetComponent<StatusManager>().StartTurn();
        }

        TakeNextTurn();
    }

    private void TakeNextTurn()
    {
        if (activeEnemyId < characterManager.enemyList.Count)
        {
            activeEnemy = characterManager.enemyList[activeEnemyId];
            activeEnemy.OnTurnFinished += ActiveEnemy_OnTurnFinished;
            activeEnemy.TakeTurn();
        }
        else
            EndEnemyTurn();
    }

    private void ActiveEnemy_OnTurnFinished()
    {
      activeEnemy.OnTurnFinished-= ActiveEnemy_OnTurnFinished;
        activeEnemyId++;
        TakeNextTurn();
    }

    private void EndEnemyTurn()
    {
        Debug.Log("Enemy Turn has Ended");
        StartPlayerTurn();
    }
}

   

