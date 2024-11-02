using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIWinLoseScreen : MonoBehaviour
{

    [SerializeField] GameObject Screen;
    [SerializeField] private TextMeshProUGUI screenText;
    [SerializeField] float WaitTime = 2;
    private Image screenBackground;


    private void Awake()
    {
        screenBackground = Screen.GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameEnd.OnGameWon += GameEnd_OnGameWon;
        GameEnd.OnGameLost += GameEnd_OnGameLost;
    }

    private void OnDisable()
    {
        GameEnd.OnGameWon -= GameEnd_OnGameWon;
        GameEnd.OnGameLost -= GameEnd_OnGameLost;
    }

    private void GameEnd_OnGameLost()
    {
        StartCoroutine(DelayedActivate(Screen));
        screenBackground.color = Color.red;
        screenText.text = "You Lost!!!";
    }



    IEnumerator DelayedActivate(GameObject screen)
    {

        yield return new WaitForSeconds(WaitTime);

        screen.SetActive(true);


    }

    private void GameEnd_OnGameWon()
    {
        StartCoroutine(DelayedActivate(Screen));
        screenBackground.color = Color.blue;
        screenText.text = "You Won!!!";
    }
}
