using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerTest : MonoBehaviour
{

    [ReadOnly] CharacterCombat testCombat;
    [SerializeField] PowerSO testPower;
    [SerializeField] bool targetObject;
    [SerializeField] ObjectHealth target;
    [SerializeField] ObjectHealth objectHealth;
    [SerializeField] Transform characterLocations;
    [SerializeField] GameObject button;


    private void Start()
    {
        if(SpawnPointController.Instance)
        SpawnPointController.Instance.SetupStage(1);

        SetupButton();
    }

    private CharacterCombat FindActiveCharacter()
    {
        CharacterCombat[] characterArray = characterLocations.GetComponentsInChildren<CharacterCombat>();
        if (characterArray.Length > 1)
            Debug.Log("More than one active character, activating the first");
        if (characterArray.Length == 1)
            return characterArray[0];

        Debug.Log("No active characters in scene");
        return null;
    }

    public void OnButtonPressed()
    {

        if (!testPower)
        {
            Debug.Log("No power set");
            return;
        }
        testCombat = FindActiveCharacter();

        if (!testCombat)
            return;

        if (targetObject)
            testCombat.StartAttack(testPower, objectHealth);
        else
            testCombat.StartAttack(testPower, target);

        SetupButton();
    }

    private void SetupButton()
    {
        button.GetComponent<Image>().sprite = testPower.icon;
        button.GetComponentInChildren<TMP_Text>().text = testPower.buttonName;
    }
}
