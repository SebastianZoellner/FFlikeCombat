using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMomentumBar : MonoBehaviour
{

    [SerializeField] Image negativeBar;
    [SerializeField] Image positiveBar;

    private void OnEnable()
    {
        MomentumManager.OnMomentumChanged += MomentumManager_OnMomentumChanged;
    }


    
    private void OnDisable()
    {
        MomentumManager.OnMomentumChanged -= MomentumManager_OnMomentumChanged;
    }
    

    private void MomentumManager_OnMomentumChanged(float momentum)
    {
        if (momentum < 0) {
            positiveBar.fillAmount = 0;
            negativeBar.fillAmount = -momentum / 100;
        }
        else
        {
            positiveBar.fillAmount = momentum / 100;
            negativeBar.fillAmount = 0;
        }
    }
}
