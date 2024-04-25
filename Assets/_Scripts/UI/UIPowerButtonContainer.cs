using UnityEngine;

public class UIPowerButtonContainer : MonoBehaviour
{
    private CharacterManager characterManager;
    [SerializeField] GameObject powerButtonContainer;
    [SerializeField] GameObject powerButtonObject;

    private UIPowerButton selectedPower=null;
    private CharacterHealth selectedHero;
    
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
        CharacterInitiative.OnAttackReadied -= CharacterInitiative_OnAttackReadied;
    }

    private void CharacterManager_OnPlayerSelectedChanged(CharacterHealth health)
    {
       
        if (!health)
        {
            HideDisplay();
            return;
        }

        selectedHero = health;

        Bind();

        powerButtonContainer.SetActive(true);

        SpawnPowerButtons();
    }

    public void SetSelectedPower(UIPowerButton power)
    {
        if (selectedPower)
            selectedPower.DeselectPower();

        selectedPower = power;
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
        HideDisplay();
    }

    private void HideDisplay()
    {
        UnBind();
        powerButtonContainer.SetActive(false);
    }

    private void Bind()
    {
        selectedHero.OnHealthChanged += SpawnPowerButtons;
    }

    private void UnBind()
    {
        if (!selectedHero)
            return;

        selectedHero.OnHealthChanged -= SpawnPowerButtons;
    }

    private void SpawnPowerButtons()
    {
        ClearPowers();

        PowerSO[] availablePowerArray = selectedHero.Stats.GetAvailablePowers(true);

        foreach (PowerSO pow in availablePowerArray)
        {
            GameObject newButton = Instantiate(powerButtonObject, powerButtonContainer.transform);

            newButton.GetComponent<UIPowerButton>().Setup(pow, characterManager, this);
            newButton.GetComponent<PowerHoverTip>().SetPower(pow);
        }
    }
}
