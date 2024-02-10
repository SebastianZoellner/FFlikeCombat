using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    public event Action<CharacterInitiative> OnActionSequenceChanged = delegate { };
    public event Action<CharacterInitiative> OnCharacterRemoved = delegate { };
    public event Action<CharacterInitiative> OncharacterAdded = delegate { };//no use for this right now, but maybe later
    public static event Action <int> OnNewRoundStarted=delegate{};

    [SerializeField] CharacterManager characterManager;
    [SerializeField] float actionSpeed = 0.02f;
    public float actionTime { get; private set; }
    private CharacterInitiative nextActor;
    public bool OngoingAction { get; private set; }

    private List<CharacterInitiative> characterInitiativeList;
    private bool listInitialized = false;

    private int round;
    
    private void OnEnable()
    {
        CharacterCombat.OnAnyActionFinished += ActionFinished;
        CharacterInitiative.OnAttackReadied += CharacterInitiative_OnAttackReadied; 
        CharacterHealth.OnAnyPCDied += Remove_Character;
        CharacterHealth.OnAnyEnemyDied += Remove_Character;
    }

    private void Start()
    {
        if(!listInitialized) InitializeCharacterList();

        nextActor=FindNextActor();
        round = 1;
        OnNewRoundStarted.Invoke(round);
    }

    private void Update()
    {
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
            ++round;
            OnNewRoundStarted.Invoke(round);
            
        }
    }

    private void OnDisable()
    {
        CharacterCombat.OnAnyActionFinished -= ActionFinished;
        CharacterInitiative.OnAttackReadied -= CharacterInitiative_OnAttackReadied;
        CharacterHealth.OnAnyPCDied -= Remove_Character;
        CharacterHealth.OnAnyEnemyDied -= Remove_Character;
    }

    public List<CharacterInitiative> GetCharacters()
    {
        if (!listInitialized) InitializeCharacterList();

        return characterInitiativeList;
    }
    //-----------------------------------------------------------------
    //         Private Functions
    //-----------------------------------------------------------------

    private void InitializeCharacterList()
    {
        characterInitiativeList = new List<CharacterInitiative>();
        foreach (PCController pcc in characterManager.playerCharacterList)
        {
            CharacterInitiative initiative = pcc.GetComponent<CharacterInitiative>();
            initiative.InitializeInitiative();
            characterInitiativeList.Add(initiative);

        }
        foreach (EnemyController ec in characterManager.enemyList)
        {
            CharacterInitiative initiative = ec.GetComponent<CharacterInitiative>();
            initiative.InitializeInitiative();
            characterInitiativeList.Add(initiative);

        }
        listInitialized = true;
    }

    private void ActionFinished()
    {
        Debug.Log("Action finished");
        OngoingAction = false;
        nextActor = FindNextActor();
    }

    private void TakeAction(CharacterInitiative nextActor)
    {
        if (nextActor.readiedAction)
        {
            Debug.Log("Perform " + nextActor.readiedAction.buttonName);
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

        Debug.Log("Next ActionTime " + nextActor.nextActionTime + " Next Actor " + nextActor.name);
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
}
