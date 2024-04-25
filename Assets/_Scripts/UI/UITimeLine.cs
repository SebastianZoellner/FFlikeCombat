using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeLine : MonoBehaviour
{
    [SerializeField] Transform timeline;
    [SerializeField] GameObject IconObject;
    [SerializeField] GameObject RoundMarker;

    private ActionSequencer actionSequencer;
    private List<PlacedIcons> placedIconsList;

    private GameObject roundMarker;
    private float baseTime;
    private int scalingFactor = 500;

    private void Awake()
    {
        actionSequencer = FindObjectOfType<ActionSequencer>();
        placedIconsList = new List<PlacedIcons>();
    }

    private void OnEnable()
    {
        CharacterManager.OnCharacterAdded += CharacterManager_OnCharacterAdded;
        actionSequencer.OnCharacterRemoved += ActionSequencer_OnCharacterRemoved;
        CharacterInitiative.OnAttackReadied += CharacterInitiative_OnAttackReadied;
    }

   
    private void Start()
    {
        InitializeActionSequence();
        roundMarker = Instantiate(RoundMarker, timeline);      
        MoveIcon(Mathf.Ceil(baseTime), roundMarker);
    }

    private void Update()
    {
        if (actionSequencer.OngoingAction)
            return;

        baseTime = ActionSequencer.actionTime;

        foreach(PlacedIcons pi in placedIconsList)
        {
            MoveIcon(pi.initiative.nextActionTime,pi.Icon);
        }

        //Debug.Log("Mathf.Ceil(baseTime):" + Mathf.Ceil(baseTime));
        MoveIcon(Mathf.Ceil(baseTime),roundMarker);
    }

    private void OnDisable()
    {
        CharacterManager.OnCharacterAdded -= CharacterManager_OnCharacterAdded;
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
       List<CharacterInitiative> characterList = actionSequencer.GetCharacters();
        foreach (CharacterInitiative ci in characterList)
        {
            
            if(!IsPlaced(ci))
            PlaceCharacterIcon(ci);
        }
    }

    private bool IsPlaced(CharacterInitiative initiative)
    {
        foreach (PlacedIcons pi in placedIconsList)
            if (pi.initiative == initiative)
                return true;
        return false;
    }

    private void PlaceCharacterIcon(CharacterInitiative ci)
    {
        GameObject newIcon = Instantiate(IconObject, timeline);
        Image image = newIcon.GetComponent<Image>();
        image.sprite = ci.GetComponent<CharacterStats>().GetIcon();
        newIcon.GetComponent<TimelineHoverTip>().SetCharacter(ci);

        PlacedIcons newPlacedIcon;
        //newPlacedIcon.time = ci.nextActionTime;
        newPlacedIcon.Icon = newIcon;
        newPlacedIcon.initiative = ci;

        MoveIcon(newPlacedIcon.initiative.nextActionTime,newPlacedIcon.Icon);
        placedIconsList.Add(newPlacedIcon);
    }

    

    private void MoveIcon(float nextTime, GameObject icon)
    {
        Vector2 targetPosition = new Vector2((nextTime - baseTime) * scalingFactor, 0);
        RectTransform iconTransform = icon.GetComponent<RectTransform>();
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

    private void CharacterManager_OnCharacterAdded(CharacterInitiative initiative)
    {
        if (!IsPlaced(initiative))
            PlaceCharacterIcon(initiative);
    }
}

public struct PlacedIcons
    {
    public GameObject Icon;
    public CharacterInitiative initiative;
}
