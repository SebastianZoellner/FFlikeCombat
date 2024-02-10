using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerButton : MonoBehaviour
{
    [SerializeField] PowerSO power;
    [SerializeField] TMP_Text powerName;
    [SerializeField] Image icon;

    private int powerID;
    private CharacterManager characterManager;

    public void Setup (PowerSO power, CharacterManager characterManager,int powerID)
    {
        this.power = power;
        this.characterManager = characterManager;
        this.powerID = powerID;

        powerName.text = power.buttonName;
        icon.sprite = power.icon;
    }

    public void PowerButtonPressed()
    {
        Debug.Log("Calling power " + powerID);
        characterManager.PressAttackButton(powerID);
    }
}
