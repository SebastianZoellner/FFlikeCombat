using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static event Action<CharacterHealth> OnPlayerSelectedChanged = delegate { };
    public static event Action<IDamageable> OnEnemySelectedChanged = delegate { };
    public static event Action<CharacterInitiative> OnCharacterAdded = delegate { }; 
    public static event Action OnAllEnemiesDead = delegate { };

    public event Action OnHeroesDead = delegate { };
    public event Action OnEnemiesDead = delegate { };

    public List<PCController> heroList;
    public List<EnemyController> enemyList;

    [SerializeField] InputReader input;

    private PCController activeCharacter;

    private bool actionOngoing;

    //-----------------------------------------
    //      Lifecycle Methods
    //-----------------------------------------

    private void OnEnable()
    {
        InputReader.OnCharacterSelected += Input_OnCharacterSelected;
        input.OnAttackSelected += Input_OnAttackSelected;
        InputReader.OnDeselected += Input_OnDeselected;

        input.OnNextEnemy += Input_OnNextEnemy;
        CharacterHealth.OnAnyPCDied += CharacterHealth_OnAnyPCDied;
        CharacterHealth.OnAnyEnemyDied+= CharacterHealth_OnAnyEnemyDied;
        CharacterHealth.OnHeroResurrected += CharacterHealth_OnHeroResurrected;
        LevelSetup.OnNewStage += LevelSetup_OnNewStage;
    }

   

    private void OnDisable()
    {
        InputReader.OnCharacterSelected -= Input_OnCharacterSelected;
        input.OnAttackSelected -= Input_OnAttackSelected;      
        InputReader.OnDeselected -= Input_OnDeselected;
        CharacterHealth.OnAnyPCDied -= CharacterHealth_OnAnyPCDied;
        CharacterHealth.OnAnyEnemyDied -= CharacterHealth_OnAnyEnemyDied;
        LevelSetup.OnNewStage -= LevelSetup_OnNewStage;
        input.OnNextEnemy -= Input_OnNextEnemy;
    }
    //-----------------------------------------
    //      Public Methods
    //-----------------------------------------

    public PCController GetActiveCharacter() => activeCharacter;

    public bool PressAttackButton(PowerSO power)
    {
        if (actionOngoing)
            return false;
       
       // Debug.Log("Starting attack " + attackID);
        activeCharacter.SetPower(power);
        return true;
    }

    public void SetSelectedPlayer(PCController selectedPlayer)
    {
        SelectNewCharacter(selectedPlayer);
    }

    public void DeselectCharacter()
    {
        Input_OnDeselected();
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (enemy)
        {
            enemyList.Add(enemy);
            OnCharacterAdded.Invoke(enemy.GetComponent<CharacterInitiative>());
        }
    }

    public void AddHero(PCController hero)
    {
        if (hero)
        {
            heroList.Add(hero);
            OnCharacterAdded.Invoke(hero.GetComponent<CharacterInitiative>());
        }
    }


    //-----------------------------------------
    //      Private Methods
    //-----------------------------------------

    private void Input_OnAttackSelected(int attackId)
    {
        if (actionOngoing)
            return;
        PowerSO[] availablePowers = activeCharacter.stats.GetAvailablePowers(true);
        if (attackId < availablePowers.Length)
        {
            //Debug.Log("Starting attack " + attackId);
            activeCharacter.SetPower(availablePowers[attackId]);
        }
    }

    private void Input_OnCharacterSelected(IDamageable health)
    {
        if (health==null)
        {
            Input_OnDeselected();
            return;
        }

        PCController selectedPlayer = health.GetTransform().GetComponent<PCController>();

        if(!selectedPlayer)//enemy was selected
        {
            SelectEnemy(health);
            return;
        }
        return;
        //SelectNewCharacter(selectedPlayer);
    }

    private void SelectEnemy(IDamageable health)
    {
        if (activeCharacter)
        {
            activeCharacter.SetTarget(health);
            OnEnemySelectedChanged.Invoke(health);
        }
    }


    private void Input_OnDeselected()
    {
        if (!activeCharacter) return;

        activeCharacter.SetTarget(null);
       
        OnEnemySelectedChanged.Invoke(null);

      
    }

    private void SelectNewCharacter(PCController newCharacter)
    {
        if (activeCharacter)
        {
           
            activeCharacter.SetDeselected();
        }
        //Debug.Log("New Hero Character Selected " + newCharacter.stats.GetName());
        activeCharacter = newCharacter;
        
        activeCharacter.SetSelected();
        OnPlayerSelectedChanged.Invoke(newCharacter.GetComponent<CharacterHealth>());
    }
    
    private void CharacterHealth_OnAnyPCDied(CharacterHealth deadPCHealth)
    {
        PCController deadPC = deadPCHealth.GetComponent<PCController>();
        heroList.Remove(deadPC);
        if (heroList.Count == 0)
            OnHeroesDead.Invoke();
    }

    private void CharacterHealth_OnAnyEnemyDied(CharacterHealth deadEnemyHealth, CharacterCombat ignored)
    {
        EnemyController deadEnemy = deadEnemyHealth.GetComponent<EnemyController>();
        enemyList.Remove(deadEnemy);

        if (enemyList.Count == 0)
        {
            OnEnemiesDead.Invoke();
            Debug.Log("All enemies dead");
        }
    }

    private void LevelSetup_OnNewStage(int obj)
    {
        foreach(PCController pcc in heroList)
        {
            //End all stauses
            pcc.GetComponent<StatusManager>().EndAll();

            //heal some proportion of health
            //Recover some endurance
            pcc.GetComponent<CharacterHealth>().NewStage();
        }
                
    }
    private void CharacterHealth_OnHeroResurrected(CharacterHealth hero)
    {
        AddHero(hero.GetComponent<PCController>());
    }

    private void Input_OnNextEnemy()
    {
        if (!activeCharacter)
            return;

        if (enemyList.Count == 0)
            return;

        IDamageable target = activeCharacter.GetTarget();

        if (target == null)
        {
            target = enemyList[0].GetComponent<CharacterHealth>();
        }
        else
        {
            for (int i = 0; i < enemyList.Count; ++i)
            {
                if (enemyList[i].GetComponent<IDamageable>() == target)
                {
                    if (i == enemyList.Count - 1)
                        target = enemyList[0].GetComponent<CharacterHealth>();

                    else
                        target = enemyList[i + 1].GetComponent<CharacterHealth>();

                    break;
                }
            }
        }

        activeCharacter.SetTarget(target);
        OnEnemySelectedChanged.Invoke(target);

    }
}
