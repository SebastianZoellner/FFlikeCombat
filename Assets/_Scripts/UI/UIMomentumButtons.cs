using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMomentumButtons : MonoBehaviour
{
    public static Action<CharacterHealth> OnAwakeHero = delegate { };
    public static Action OnEnvigorate = delegate { };
    public static Action OnTakeAdvantage = delegate { };
    public static Action<CharacterInitiative> OnSwitchHero = delegate { };

    [SerializeField] private GameObject selectTargetScreen;
    [SerializeField] private GameObject buttonField;
    [SerializeField] private Button activateButton;
    [SerializeField] private Sprite activatedIcon;
    [SerializeField] private Sprite deactivatedIcon;


    private PCController selectedHero;

    private bool waitToSelectTarget;

    private void OnEnable()
    {
        InputReader.OnCharacterSelected += InputReader_OnCharacterSelected;
        InputReader.OnDeselected += InputReader_OnDeselected;
    }

    

    private void OnDisable()
    {
        InputReader.OnCharacterSelected -= InputReader_OnCharacterSelected;
    }

    private void Start()
    {
        buttonField.SetActive(false);
        activateButton.image.sprite = deactivatedIcon;
        selectTargetScreen.SetActive(false);
    }


    public void ActivateButtonPressed()
    {
        if (buttonField.activeInHierarchy)
        {
            buttonField.SetActive(false);
            activateButton.image.sprite = deactivatedIcon;
        }
        else
        {
            buttonField.SetActive(true);
            activateButton.image.sprite = activatedIcon;
        }
    }

    public void AwakeButtonPressed()
    {
        SelectHero(true);
        StartCoroutine(AwakeEffect());
    }

    public void EnvigorateButtonPressed()
    {
        OnEnvigorate.Invoke();
    }

    public void TakeAdvantageButtonPressed()
    {
        OnTakeAdvantage();
    }

    public void SwitchHeroButtonPressed()
    {
        SelectHero(true);
        StartCoroutine(SwitchHero());
    }


    private void SelectHero(bool select)
    {
        waitToSelectTarget = select;
        //Disable/enable other input

        //Activate screen
        selectTargetScreen.SetActive(select);
    }

    private void InputReader_OnCharacterSelected(IDamageable health)
    {
        if (!waitToSelectTarget)
            return;
        if (health==null)
            return;

        selectedHero = health.GetTransform().GetComponent<PCController>();

        if (selectedHero)
            SelectHero(false);

    }

    private IEnumerator AwakeEffect()
    {
        while (waitToSelectTarget)
            yield return null;
        CharacterHealth health = selectedHero.GetComponent<CharacterHealth>();

        if (!health.canBeTarget)
            OnAwakeHero.Invoke(health);
    }

    IEnumerator SwitchHero()
    {
        while (waitToSelectTarget)
            yield return null;

        if (selectedHero.GetComponent<CharacterHealth>().canBeTarget)
            OnSwitchHero.Invoke(selectedHero.GetComponent<CharacterInitiative>());
    }

    private void InputReader_OnDeselected()
    {
        StopAllCoroutines();
        SelectHero(false);
    }
}
