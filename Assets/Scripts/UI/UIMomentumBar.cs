using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using TMPro;

public class UIMomentumBar : MonoBehaviour
{

    [SerializeField] Image negativeBar;
    [SerializeField] Image positiveBar;

    [SerializeField] MMProgressBar negativeMBar;
    [SerializeField] MMProgressBar positiveMBar;
    [SerializeField] TextMeshProUGUI numberDisplay;

    //Test stuff
    [Range(-100, 100)]
    public float value;
    [MMInspectorButton("ChangeBarValue")] public bool ChangeBarValueButton;

    private void OnEnable()
    {
        MomentumManager.OnMomentumChanged += MomentumManager_OnMomentumChanged;
    }


    
    private void OnDisable()
    {
        MomentumManager.OnMomentumChanged -= MomentumManager_OnMomentumChanged;
    }
    
    private void ChangeBarValue()
    {
        numberDisplay.text = "[" + Mathf.RoundToInt(value).ToString() + "]";
        if (value < 0)
        {
            positiveMBar.UpdateBar(0, 0, 100);
            negativeMBar.UpdateBar(-value, 0, 100);
        }
        else
        {
            positiveMBar.UpdateBar(value, 0, 100);
            negativeMBar.UpdateBar(0, 0, 100);
        }
    }

    private void MomentumManager_OnMomentumChanged(float momentum)
    {
        numberDisplay.text = "[" + Mathf.RoundToInt(momentum).ToString() + "]";
        if (momentum < 0)
        {
            positiveMBar.UpdateBar(0, 0, 100);
            negativeMBar.UpdateBar(-momentum, 0, 100);
        }
        else
        {
            positiveMBar.UpdateBar(momentum, 0, 100);
            negativeMBar.UpdateBar(0, 0, 100);
        }
    }
}
