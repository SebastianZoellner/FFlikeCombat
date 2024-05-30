using UnityEngine;

public class UIHeroButtonContainer : MonoBehaviour
{
    [SerializeField] private StartScreenManager startScreen;
    [SerializeField] GameObject heroButtonContainer;
    [SerializeField] GameObject heroButtonObject;

    private void Start()
    {
        CharacterSO[] availableCharacters = startScreen.GetAvailableHeroes();

        foreach (CharacterSO cha in availableCharacters)
        {
            GameObject newButton = Instantiate(heroButtonObject, heroButtonContainer.transform);

            newButton.GetComponent<UIHeroSelectButton>().Setup(cha, startScreen);
        }
    }

}
