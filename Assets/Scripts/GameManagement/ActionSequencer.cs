using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    public event Action<CharacterInitiative> OnActionSequenceChanged = delegate { };
    public event Action<CharacterInitiative> OnCharacterRemoved = delegate { };
    public event Action<CharacterInitiative> OncharacterAdded = delegate { };//no use for this right now, but maybe later
    public static event Action <int> OnNewRoundStarted=delegate{}; //Fires UI, Spawns new Wave, removes fallen enemies

    [SerializeField] CharacterManager characterManager;
    [SerializeField] float actionSpeed = 0.02f;

    public float actionTime { get; private set; }
    private CharacterInitiative nextActor;
    public bool OngoingAction { get; private set; }

    private List<CharacterInitiative> characterInitiativeList;
    private bool listInitialized = false;

    private int round=0;


    private void Awake()
    {
        characterInitiativeList = new List<CharacterInitiative>();
        if (!listInitialized) InitializeCharacterList();
    }

    private void OnEnable()
    {
        CharacterCombat.OnAnyActionFinished += ActionFinished;
        CharacterInitiative.OnAttackReadied += CharacterInitiative_OnAttackReadied; 
        CharacterHealth.OnAnyPCDied += Remove_Character;
        CharacterHealth.OnAnyEnemyDied += Remove_Character;
        CharacterManager.OnCharacterAdded += CharacterManager_OnCharacterAdded;
        characterManager.OnWaveDefeated += CharacterManager_OnWaveDefeated;
    }

   

    private void Update()
    {
        if(!nextActor)
            nextActor = FindNextActor();

        if (OngoingAction)
            return;

        actionTime += Time.deltaTime * actionSpeed;
        if (actionTime > nextActor.nextActionTime)
        {
            Debug.Log("New Actor starting " + nextActor.name);
            OngoingAction = true;
            TakeAction(nextActor);           
        }

        if(actionTime>round)
        {
            StartNewRound();
        }
    }

    

    private void OnDisable()
    {
        CharacterCombat.OnAnyActionFinished -= ActionFinished;
        CharacterInitiative.OnAttackReadied -= CharacterInitiative_OnAttackReadied;
        CharacterHealth.OnAnyPCDied -= Remove_Character;
        CharacterHealth.OnAnyEnemyDied -= Remove_Character;
        CharacterManager.OnCharacterAdded -= CharacterManager_OnCharacterAdded;
        characterManager.OnWaveDefeated -= CharacterManager_OnWaveDefeated;
    }

    public List<CharacterInitiative> GetCharacters()
    {
        if (!listInitialized) InitializeCharacterList();

        return characterInitiativeList;
    }
    //-----------------------------------------------------------------
    //         Private Functions
    //-----------------------------------------------------------------

    private void AddCharacter(CharacterInitiative initiative)
    {
        //if (!initiative) return;       
          initiative.InitializeInitiative(actionTime);
        characterInitiativeList.Add(initiative);

        if (!nextActor ||initiative.nextActionTime < nextActor.nextActionTime)
            nextActor = initiative;
    }

    private void InitializeCharacterList()
    {
       
        foreach (PCController pcc in characterManager.heroList)
        {
            CharacterInitiative initiative = pcc.GetComponent<CharacterInitiative>();
            AddCharacter(initiative);

        }
        foreach (EnemyController ec in characterManager.enemyList)
        {
            CharacterInitiative initiative = ec.GetComponent<CharacterInitiative>();
            AddCharacter(initiative);

        }
        listInitialized = true;
    }

    private void ActionFinished()
    {
       // Debug.Log("Action finished");
        OngoingAction = false;
        nextActor = FindNextActor();
    }

    private void TakeAction(CharacterInitiative nextActor)
    {
        if (nextActor.readiedAction)
        {
            //Debug.Log("Perform " + nextActor.readiedAction.buttonName);           
            nextActor.PerformReadiedAction();
            return;
        }

        PCController pcController = nextActor.GetComponent<PCController>();

        if (pcController)
        {
            //Debug.Log("Player Turn");
            characterManager.SetSelectedPlayer(pcController);
            return;
        }
        EnemyController enemyController = nextActor.GetComponent<EnemyController>();
        //Debug.Log("Enemy Turn");
        enemyController.SetNextAction();

    }

    private CharacterInitiative FindNextActor()
    {
        CharacterInitiative nextActor = null;

        foreach (CharacterInitiative ci in characterInitiativeList)
        {
            if (nextActor == null || ci.nextActionTime < nextActor.nextActionTime)
                nextActor = ci;
        }
        if (nextActor)
            Debug.Log("Next ActionTime " + nextActor.nextActionTime + " Next Actor " + nextActor.name);
        else
            Debug.Log("No next Actor set");
        return nextActor;
    }

    private void Remove_Character(CharacterHealth health)
    {
        CharacterInitiative deadCharacter = health.GetComponent<CharacterInitiative>();
        characterInitiativeList.Remove(deadCharacter);
        OnCharacterRemoved.Invoke(deadCharacter);
    }

    private void CharacterInitiative_OnAttackReadied(bool isReadied, CharacterInitiative arg2)
    {
        if (isReadied)
            ActionFinished();
    }

    private void CharacterManager_OnCharacterAdded(CharacterInitiative initiative)
    {
        AddCharacter(initiative);
    }

    

    private void CharacterManager_OnWaveDefeated()
    {
        foreach(CharacterInitiative ci in characterInitiativeList)
        {
            if (ci.nextActionTime < round)
                ci.SetNextActionTime(round + ci.nextActionTime / 100);
        }
    }

    private void StartNewRound()
    {
        ++round;
        Debug.Log("New Round " + round);
        OnNewRoundStarted.Invoke(round);
    }
}
