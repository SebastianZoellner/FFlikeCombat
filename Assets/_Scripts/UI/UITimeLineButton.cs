using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeLineButton : MonoBehaviour
{
    [SerializeField] GameObject ReadyIndicator;

    private void Start()
    {
        ReadyIndicator.SetActive(false);
    }

    public void  SetReadyIndicator(bool indicator)
    {
        ReadyIndicator.SetActive(indicator);
    }
}
