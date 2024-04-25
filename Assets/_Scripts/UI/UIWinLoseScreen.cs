using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIWinLoseScreen : MonoBehaviour
{
   
    [SerializeField] GameObject Screen;
    [SerializeField] private TextMeshProUGUI screenText;
    [SerializeField] float WaitTime=2;
    private Image screenBackground;
    

    private void Awake()
    {
        screenBackground = Screen.GetComponent<Image>();
    }

    private void OnEnable()
    {
        LevelSetup.LevelWon += LevelSetup_LevelWon;
        LevelSetup.OnGameLost += LevelSetup_OnGameLost;
    }

    private void OnDisable()
    {
        LevelSetup.LevelWon -= LevelSetup_LevelWon;
        LevelSetup.OnGameLost -= LevelSetup_OnGameLost;
    }

    private void LevelSetup_OnGameLost()
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

    private void LevelSetup_LevelWon()
    {
        StartCoroutine(DelayedActivate(Screen));
        screenBackground.color = Color.blue;
        screenText.text = "You Won!!!";
    }
}
