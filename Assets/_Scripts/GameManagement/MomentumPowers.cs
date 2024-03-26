using System;
using UnityEngine;

public class MomentumPowers : MonoBehaviour
{
    private static readonly float AwakeCost = 50f;
    private static readonly float EnvigorateCost = 10f;
    private static readonly float TakeAdvantageCost = 5f;
    private static readonly float SwitchInitiativeCost = 5f;

    private MomentumManager momentumManager;
    [SerializeField] CharacterManager characterManager;
    [SerializeField] ActionSequencer actionSequencer;

    private void Awake()
    {
        momentumManager = GetComponent<MomentumManager>();
    }
    private void OnEnable()
    {
        UIMomentumButtons.OnAwakeHero += AwakeHero;
        UIMomentumButtons.OnEnvigorate += Envigorate;
        UIMomentumButtons.OnTakeAdvantage += TakeAdvantage;
        UIMomentumButtons.OnSwitchHero += SwitchInitiative;
    }

    private void OnDisable()
    {
        UIMomentumButtons.OnAwakeHero -= AwakeHero;
        UIMomentumButtons.OnEnvigorate -= Envigorate;
        UIMomentumButtons.OnTakeAdvantage -= TakeAdvantage;
        UIMomentumButtons.OnSwitchHero -= SwitchInitiative;
    }

    public void AwakeHero(CharacterHealth downedHero)
    {
        if (!momentumManager.PayMomentum(AwakeCost))
            return;

        downedHero.Raise();      
    }

    public void Envigorate()
    {
        if(!momentumManager.PayMomentum(EnvigorateCost))
            return;
        PCController activeCharacter = characterManager.GetActiveCharacter();
        if(activeCharacter)
        activeCharacter.GetComponent<CharacterHealth>().Envigorate();
    }

    public void TakeAdvantage()
    {
        if (MomentumManager.GetMomentum() < TakeAdvantageCost)
            return;

        momentumManager.PayMomentum(TakeAdvantageCost);

        PCController activeCharacter = characterManager.GetActiveCharacter();
        if (activeCharacter)
            activeCharacter.GetComponent<StatusManager>().GainStatus(StatusName.TacticalAdvantage,0,0,0);
    }

    public void SwitchInitiative(CharacterInitiative otherHero)
    {
        if (!momentumManager.PayMomentum(SwitchInitiativeCost))
            return;

        PCController activeCharacter = characterManager.GetActiveCharacter();

        if (activeCharacter)
            actionSequencer.SwitchCharacters(activeCharacter.GetComponent<CharacterInitiative>(), otherHero);
    }
}
