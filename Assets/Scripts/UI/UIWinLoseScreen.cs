using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinLoseScreen : MonoBehaviour
{
    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject WinScreen;
    [SerializeField] float WaitTime=2;

    private void OnEnable()
    {
        CharacterManager.OnAllEnemiesDead += CharacterManager_OnAllEnemiesDead;
        CharacterManager.OnAllHeroesDead += CharacterManager_OnAllHeroesDead;
    }

    private void OnDisable()
    {
        CharacterManager.OnAllEnemiesDead -= CharacterManager_OnAllEnemiesDead;
        CharacterManager.OnAllHeroesDead -= CharacterManager_OnAllHeroesDead;
    }

    private void CharacterManager_OnAllHeroesDead()
    {
        StartCoroutine(DelayedActivate(LoseScreen));
    }

    private void CharacterManager_OnAllEnemiesDead()
    {
        StartCoroutine(DelayedActivate(WinScreen));
    }

    private void StopGame()
    {
        Time.timeScale = 0f;
    }

    IEnumerator DelayedActivate(GameObject screen)
    {
        yield return new WaitForSeconds(WaitTime);

        screen.SetActive(true);
        StopGame();

    }
}
