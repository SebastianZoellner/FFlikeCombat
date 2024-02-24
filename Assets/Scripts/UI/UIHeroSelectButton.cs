using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroSelectButton : MonoBehaviour
{
    
    [SerializeField] TMP_Text characterName;
    [SerializeField] Image icon;
    [SerializeField] Image frame;

    private bool isSelected = false;

    private CharacterSO character;
    private StartScreenManager startScreen;

    public void Setup(CharacterSO character, StartScreenManager startScreen)
    {
        this.character = character;
        this.startScreen = startScreen;


        characterName.text = character.CharacterName;
        icon.sprite = character.icon;
    }

    public void HeroButtonPressed()
    {
        bool rv= startScreen.PressCharacterSelectButton(character);

        if (!rv) return;

        if (isSelected)
        {

            frame.color = Color.white;
            isSelected = false;
        }
        else
        {
            frame.color = Color.red;
            isSelected = true;
        }

       
    }


}
