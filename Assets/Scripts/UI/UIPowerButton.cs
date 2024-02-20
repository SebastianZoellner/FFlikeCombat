using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerButton : MonoBehaviour
{
    [SerializeField] PowerSO power;
    [SerializeField] TMP_Text powerName;
    [SerializeField] Image icon;

    
    private CharacterManager characterManager;

    public void Setup (PowerSO power, CharacterManager characterManager)
    {
        this.power = power;
        this.characterManager = characterManager;
        

        powerName.text = power.buttonName;
        icon.sprite = power.icon;
    }

    public void PowerButtonPressed()
    {
        //Debug.Log("Calling power " + powerID);
        characterManager.PressAttackButton(power);
    }
}
