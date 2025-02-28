using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroSelectButton : MonoBehaviour
{  
    [SerializeField] TMP_Text characterName;
    [SerializeField] Image icon;
    [SerializeField] RawImage scanLines;
    [SerializeField] Image frame;
    [SerializeField] Animator imageAnimator;

    private bool isSelected = false;

    private CharacterSO character;
    private StartScreenManager startScreen;

    private Color unselectedScanlinesColor=new Color(30f/255,184f/255,1f,51f/255);
    private Color unselectedFrameColor = new Color(30f / 255, 184f / 255, 1f, 1);
    private Color selectedScanlinesColor=new Color(0,1,0.3f,0.35f);
    private Color selectedFrameColor = new Color(0, 1, 0.3f, 1);

    public void Setup(CharacterSO character, StartScreenManager startScreen)
    {
        this.character = character;
        this.startScreen = startScreen;


        characterName.text = character.CharacterName;
        icon.sprite = character.icon;
        scanLines.color = unselectedScanlinesColor;
        frame.color = unselectedFrameColor;
        
        StartCoroutine(WaitToAnimation());
    }

    public void HeroButtonPressed()
    {
        bool rv= startScreen.PressCharacterSelectButton(character);

        if (!rv) return;

        if (isSelected)
        {
            frame.color = unselectedFrameColor;
            scanLines.color = unselectedScanlinesColor;
            isSelected = false;
        }
        else
        {
            frame.color = selectedFrameColor;
            scanLines.color = selectedScanlinesColor;
            isSelected = true;
        }     
    }

     IEnumerator WaitToAnimation()
        {
        float waitTime = Random.Range(0, 10f);
        Debug.Log("Wait Time "+waitTime);
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Setting Start");
        imageAnimator.SetTrigger("Start");
        }

}
