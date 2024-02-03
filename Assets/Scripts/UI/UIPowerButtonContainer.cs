using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPowerButtonContainer : MonoBehaviour
{
    private CharacterManager characterManager;
    [SerializeField] GameObject powerButtonContainer;
    [SerializeField] GameObject powerButtonObject;
    
    private void Awake()
    {
        characterManager = FindObjectOfType<CharacterManager>();
    }
    private void OnEnable()
    {
        CharacterManager.OnPlayerSelectedChanged += CharacterManager_OnPlayerSelectedChanged;
    }

    private void OnDisable()
    {
        CharacterManager.OnPlayerSelectedChanged -= CharacterManager_OnPlayerSelectedChanged;
    }

    private void CharacterManager_OnPlayerSelectedChanged(CharacterHealth health)
    {
        ClearPowers();
        if (!health)
        {
            powerButtonContainer.SetActive(false);
            return;
        }

        powerButtonContainer.SetActive(true);

        int numberOfPowers = health.Stats.GetNumbeOfPowers();

        for(int i = 0; i < numberOfPowers; ++i)
        {
            GameObject newButton=Instantiate(powerButtonObject, powerButtonContainer.transform);

            newButton.GetComponent<UIPowerButton>().Setup(health.Stats.GetPower(i),characterManager, i);
        }
    }

    private void ClearPowers()
    {
       foreach (Transform tr in powerButtonContainer.transform)
        {
            Destroy(tr.gameObject);
        }
    }
}
