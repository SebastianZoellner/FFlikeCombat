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
        CharacterInitiative.OnAttackReadied += CharacterInitiative_OnAttackReadied;
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

        PowerSO[] availablePowerArray = health.Stats.GetAvailablePowers();

        foreach(PowerSO pow in availablePowerArray)
        {
            GameObject newButton=Instantiate(powerButtonObject, powerButtonContainer.transform);

            newButton.GetComponent<UIPowerButton>().Setup(pow,characterManager);
        }
    }

    private void ClearPowers()
    {
       foreach (Transform tr in powerButtonContainer.transform)
        {
            Destroy(tr.gameObject);
        }
    }

    private void CharacterInitiative_OnAttackReadied(bool arg1, CharacterInitiative arg2)
    {
        powerButtonContainer.SetActive(false);
    }
}
