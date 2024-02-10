using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeLine : MonoBehaviour
{
    [SerializeField] Transform timeline;
    [SerializeField] GameObject IconObject;

    private ActionSequencer actionSequencer;
    private List<PlacedIcons> placedIconsList;

    private float baseTime;
    private int scalingFactor = 500;

    private void Awake()
    {
        actionSequencer = FindObjectOfType<ActionSequencer>();
    }

    private void OnEnable()
    {
        
        actionSequencer.OnCharacterRemoved += ActionSequencer_OnCharacterRemoved;
        CharacterInitiative.OnAttackReadied += CharacterInitiative_OnAttackReadied;
    }

    private void Start()
    {
        InitializeActionSequence();
    }

    private void Update()
    {
        if (actionSequencer.OngoingAction)
            return;

        baseTime = actionSequencer.actionTime;

        foreach(PlacedIcons pi in placedIconsList)
        {
            MoveIcon(pi);
        }
    }

    private void OnDisable()
    {
        actionSequencer.OnCharacterRemoved -= ActionSequencer_OnCharacterRemoved;
        CharacterInitiative.OnAttackReadied -= CharacterInitiative_OnAttackReadied;
    }

    private void ActionSequencer_OnCharacterRemoved(CharacterInitiative character)
    {
        foreach (PlacedIcons pi in placedIconsList)
        {
            if (pi.initiative != character)
                continue;

            Destroy(pi.Icon);
            placedIconsList.Remove(pi);
            break;
        }
    }


    private void InitializeActionSequence() 
    {
        placedIconsList = new List<PlacedIcons>();

       List<CharacterInitiative> characterList = actionSequencer.GetCharacters();
        foreach (CharacterInitiative ci in characterList)
        {
            GameObject newIcon = Instantiate(IconObject, timeline);
            Image image = newIcon.GetComponent<Image>();
            image.sprite = ci.GetComponent<CharacterStats>().GetIcon();
            
            PlacedIcons newPlacedIcon;
            //newPlacedIcon.time = ci.nextActionTime;
            newPlacedIcon.Icon = newIcon;
            newPlacedIcon.initiative = ci;

            MoveIcon(newPlacedIcon);
            placedIconsList.Add(newPlacedIcon);
        }
    
    }
    private void MoveIcon(PlacedIcons icon)
    {
        Vector2 targetPosition = new Vector2((icon.initiative.nextActionTime - baseTime) * scalingFactor, 0);
        RectTransform iconTransform = icon.Icon.GetComponent<RectTransform>();
        iconTransform.anchoredPosition = targetPosition;
    }
    private void CharacterInitiative_OnAttackReadied(bool readied, CharacterInitiative character)
    {
        foreach (PlacedIcons pi in placedIconsList)
        {
            if (pi.initiative != character)
                continue;

            pi.Icon.GetComponent<UITimeLineButton>().SetReadyIndicator(readied);
            break;
        }
    }
}

public struct PlacedIcons
    {
    //public float time;
    public GameObject Icon;
    public CharacterInitiative initiative;
}
