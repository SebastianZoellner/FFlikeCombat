using System;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public static event Action OnGameWon = delegate { };
    public static event Action OnGameLost = delegate { };

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private MomentumManager momentumManager;

    private void OnEnable()
    {
        characterManager.OnHeroesDead += LoseGame;
        momentumManager.OnMomentumLoss += LoseGame;
        momentumManager.OnMomentumWin += WinGame;    
    }


    private void OnDisable()
    {
        characterManager.OnHeroesDead -= LoseGame;
        momentumManager.OnMomentumLoss -= LoseGame;
        momentumManager.OnMomentumWin -= WinGame;
    }

    private void LoseGame()
    {
        OnGameLost.Invoke();
    }

    public void WinGame()
    {
        OnGameWon.Invoke();
    }

}
