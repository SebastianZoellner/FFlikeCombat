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
    public static event Action<CharacterInitiative> OnNextActorStarting = delegate { };//adjusts camera
    public static event Action OnNoActor = delegate { };
   
    public static float actionTime { get; private set; }


    [SerializeField] CharacterManager characterManager;
    
    [SerializeField] float actionSpeed = 0.02f;

    
    private CharacterInitiative nextActor;
    public bool OngoingAction { get; private set; }

    public List<CharacterInitiative> characterInitiativeList;
    private bool listInitialized = false;

    private int round=0;
    
    private bool isWaiting=true;
    private bool isStopped = false;
    private float startStageWait=2f;

    private void OnEnable()
    {
        UIMissionBriefing.OnBriefingSet += UIMissionBriefing_OnBriefingSet;
        LevelSetup.OnNewStage += LevelSetup_OnNewStage;
        CharacterCombat.OnAnyActionFinished += ActionFinished;
        CharacterInitiative.OnAttackReadied += CharacterInitiative_OnAttackReadied;
        CharacterInitiative.OnActionTimeChanged += CharacterInitiative_OnActionTimeChanged;
        CharacterHealth.OnAnyPCDied += Remove_Character;
        CharacterHealth.OnAnyEnemyDied += CharacterHealth_OnAnyEnemyDied;      
        CharacterManager.OnCharacterAdded += CharacterManager_OnCharacterAdded;
        characterManager.OnEnemiesDead += CharacterManager_OnWaveDefeated;
        GameEnd.OnGameLost += StopActionSequence;
        GameEnd.OnGameWon += StopActionSequence;
        LevelSetup.OnGameInitialized += LevelSetup_OnGameInitialized;
    }

    

    private void Update()
    {
        if (isWaiting||isStopped)
            return;

       
        if (!nextActor)
            nextActor = FindNextActor();

        if (OngoingAction)
            return;

        actionTime += Time.deltaTime * actionSpeed;
        if (actionTime > nextActor.nextActionTime)
        {
            //Debug.Log("New Actor starting " + nextActor.name);
            OngoingAction = true;
            OnNextActorStarting.Invoke(nextActor);
            TakeAction(nextActor);           
        }

        if(actionTime>round)
        {
            StartNewRound();           
        }
    }

    

    private void OnDisable()
    {
        UIMissionBriefing.OnBriefingSet -= UIMissionBriefing_OnBriefingSet;
        LevelSetup.OnNewStage -= LevelSetup_OnNewStage;
        CharacterCombat.OnAnyActionFinished -= ActionFinished;
        CharacterInitiative.OnAttackReadied -= CharacterInitiative_OnAttackReadied;
        CharacterInitiative.OnActionTimeChanged -= CharacterInitiative_OnActionTimeChanged;
        CharacterHealth.OnAnyPCDied -= Remove_Character;
        CharacterHealth.OnAnyEnemyDied -= CharacterHealth_OnAnyEnemyDied;
        CharacterManager.OnCharacterAdded -= CharacterManager_OnCharacterAdded;
        characterManager.OnEnemiesDead -= CharacterManager_OnWaveDefeated;

        GameEnd.OnGameLost -= StopActionSequence;
        GameEnd.OnGameWon -= StopActionSequence;
        LevelSetup.OnGameInitialized -= LevelSetup_OnGameInitialized;
    }

public void SwitchCharacters(CharacterInitiative characterInitiative1, CharacterInitiative characterInitiative2)
    {
        float timeStore = characterInitiative1.nextActionTime;
        characterInitiative1.SetNextActionTime(characterInitiative2.nextActionTime + 0.001f);
        characterInitiative2.SetNextActionTime(timeStore + 0.001f);

        ActionFinished();
        
    }

   

    public List<CharacterInitiative> GetCharacters()
    {
        if (!listInitialized) InitializeCharacterList();

        return characterInitiativeList;
    }
    //-----------------------------------------------------------------
    //         Private Functions
    //-----------------------------------------------------------------

    private void LevelSetup_OnGameInitialized()
    {
        isWaiting = false;
        listInitialized = false;
        round = 0;
        actionTime = 0;
        characterInitiativeList = new List<CharacterInitiative>();
        InitializeCharacterList();
    }

    private void AddCharacter(CharacterInitiative initiative)
    {
       Debug.Log("Adding " + initiative.name);

          initiative.InitializeInitiative(actionTime);
        characterInitiativeList.Add(initiative);

        if (!nextActor ||initiative.nextActionTime < nextActor.nextActionTime)
            nextActor = initiative;
    }

    private void InitializeCharacterList()
    {
        if (listInitialized)
            return;

        Debug.Log("Initializing Character List");
       
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
        OnNoActor.Invoke();
        nextActor = FindNextActor();
        characterManager.DeselectCharacter();
    }

    private void TakeAction(CharacterInitiative nextActor)
    {
        if (nextActor.HasReadiedAction())
        {
            //Debug.Log("Perform " + nextActor.readiedAction.buttonName);           
            if (!nextActor.PerformReadiedAction())
                ActionFinished();

            return;
        }

        PCController pcController = nextActor.GetComponent<PCController>();

        if (pcController)
        {
            characterManager.SetSelectedPlayer(pcController);
            return;
        }
        EnemyController enemyController = nextActor.GetComponent<EnemyController>();
 
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
       /* if (nextActor)
            Debug.Log("Next ActionTime " + nextActor.nextActionTime + " Next Actor " + nextActor.name);
        else
            Debug.Log("No next Actor set");*/
        return nextActor;
    }

    private void CharacterHealth_OnAnyEnemyDied(CharacterHealth enemy, CharacterCombat ignored)
    {
        Remove_Character(enemy);
    }

    private void Remove_Character(CharacterHealth health)
    {
        CharacterInitiative deadCharacter = health.GetComponent<CharacterInitiative>();
        characterInitiativeList.Remove(deadCharacter);
        //Debug.Log("Removing From Inititiative list" + deadCharacter.name);
        FindNextActor();
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

    private void CharacterInitiative_OnActionTimeChanged()
    {
        FindNextActor();
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
        StartCoroutine(Wait(2));
        OnNewRoundStarted.Invoke(round);

       foreach(CharacterInitiative ci in characterInitiativeList)      
            Debug.Log(ci.name + " next Action time: " + ci.nextActionTime);
        
    }

    private void LevelSetup_OnNewStage(int obj)
    {
        StartCoroutine(Wait(startStageWait));
        round = 0;
        actionTime = 0;

        foreach (PCController pcc in characterManager.heroList)
            pcc.GetComponent<CharacterInitiative>().InitializeInitiative(actionTime);

    }

    private void UIMissionBriefing_OnBriefingSet(bool briefingSet)
    {
        isStopped = briefingSet;
       
    }

    private void StopActionSequence()
    {
        isStopped = true; ;
    }

    private IEnumerator Wait(float waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
