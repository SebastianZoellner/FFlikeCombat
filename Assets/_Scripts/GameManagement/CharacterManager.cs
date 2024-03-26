using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static event Action<CharacterHealth> OnPlayerSelectedChanged = delegate { };
    public static event Action<CharacterHealth> OnEnemySelectedChanged = delegate { };
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
        PowerSO[] availablePowers = activeCharacter.stats.GetAvailablePowers();
        if (attackId < availablePowers.Length)
        {
            //Debug.Log("Starting attack " + attackId);
            activeCharacter.SetPower(availablePowers[attackId]);
        }
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
        return;
        //SelectNewCharacter(selectedPlayer);
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
       // activeCharacter.SetDeselected();

       // OnPlayerSelectedChanged.Invoke(null);
        OnEnemySelectedChanged.Invoke(null);

        //activeCharacter = null;
    }

    private void SelectNewCharacter(PCController newCharacter)
    {
        if (activeCharacter)
        {
            activeCharacter.OnActionStarted -= ActiveCharacter_OnActionStarted;
            activeCharacter.OnActionEnded -= ActiveCharacter_OnActionEnded;
            activeCharacter.SetDeselected();
        }
        //Debug.Log("New Hero Character Selected " + newCharacter.stats.GetName());
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
        heroList.Remove(deadPC);
        if (heroList.Count == 0)
            OnHeroesDead.Invoke();
    }

    private void CharacterHealth_OnAnyEnemyDied(CharacterHealth deadEnemyHealth)
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

        }
                
    }
    private void CharacterHealth_OnHeroResurrected(CharacterHealth hero)
    {
        AddHero(hero.GetComponent<PCController>());
    }
}
