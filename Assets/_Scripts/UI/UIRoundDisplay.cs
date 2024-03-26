using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRoundDisplay : MonoBehaviour
{
    [SerializeField] GameObject roundCountDisplay;
    private TextMeshProUGUI text;
    private float displayTime = 1f;

    private void Awake()
    {
        text = roundCountDisplay.GetComponentInChildren<TextMeshProUGUI>();
    }
    private void  OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }

    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }
    private void ActionSequencer_OnNewRoundStarted(int round)
    {
        roundCountDisplay.SetActive(true);
        text.text = "Round " + round;
        StartCoroutine(DelayedInactivate());
    }

    private IEnumerator DelayedInactivate()
    {
        yield return new WaitForSeconds(displayTime);
        roundCountDisplay.SetActive(false);
    }
}
