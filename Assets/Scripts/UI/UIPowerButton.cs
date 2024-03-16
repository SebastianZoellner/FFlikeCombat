using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerButton : MonoBehaviour
{
    [SerializeField] PowerSO power;
    [SerializeField] TMP_Text powerName;
    [SerializeField] Image icon;
    [SerializeField] Image frame;
    private Color notSelected = Color.white;
    private Color selected = Color.red;
    private UIPowerButtonContainer buttonContainer;

    
    private CharacterManager characterManager;


    public void Setup (PowerSO power, CharacterManager characterManager, UIPowerButtonContainer buttonContainer)
    {
        this.power = power;
        this.characterManager = characterManager;
        this.buttonContainer = buttonContainer;
        

        powerName.text = power.buttonName;
        icon.sprite = power.icon;
        frame.color = notSelected;
        
    }

    public void PowerButtonPressed()
    {
        //Debug.Log("Calling power " + powerID);
        characterManager.PressAttackButton(power);
        buttonContainer.SetSelectedPower(this);
        frame.color = selected;
    }

    public void DeselectPower()
    {
        frame.color = notSelected;
    }
}
