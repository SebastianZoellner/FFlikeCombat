using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIInfoScreen : MonoBehaviour
{

    [SerializeField] GameObject infoScreen;
    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text blurb;
    [SerializeField] TMP_Text attributes;
    [SerializeField] TMP_Text otherAttributes;
    [SerializeField] TMP_Text healthScore;
    [SerializeField] TMP_Text statuses;
    private void OnEnable()
    {
        InputReader.OnSelectedEntityChanged += InputReader_OnSelectedEntityChanged;
    }

    private void OnDisable()
    {
        InputReader.OnSelectedEntityChanged -= InputReader_OnSelectedEntityChanged;
    }

    private void InputReader_OnSelectedEntityChanged(Entity entity)
    {
        if (!entity)
        {
            infoScreen.SetActive(false);
            return;
        }

        infoScreen.SetActive(true);

        characterName.text = entity.Stats.GetName();
        SetHealthParameters(entity.Health);
        SetAttributes(entity.Health.Stats);
        SetStatuses(entity.StatusManager);

    }

    private void SetAttributes(CharacterStats stats)
    {
        attributes.text = "";
        attributes.text += "Combat  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Combat))+"\n";
        attributes.text += "Power  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Power)) + "\n";
        attributes.text += "Agillity  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Agillity)) + "\n";
        attributes.text += "Armor  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Armor)) + "\n";

        otherAttributes.text = "";
        otherAttributes.text += "Speed  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Speed)) + "\n";
        otherAttributes.text += "Initiative  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Initiative)) + "\n";
        otherAttributes.text += "Hardiness  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Hardiness)) + "\n";
        otherAttributes.text += "Recovery  " + Mathf.RoundToInt(stats.GetAttribute(Attribute.Recovery)) + "\n";

    }

    private void SetStatuses(StatusManager statusManager)
    {
        string[] statusArray = statusManager.GetStatusNames();

        statuses.text = "";

        for (int i = 0; i < statusArray.Length; ++i)
        {
            statuses.text += statusArray[i] + "\n";
        }
    }

    private void SetHealthParameters(CharacterHealth health)
    {
        int maxHealth = Mathf.RoundToInt(health.StartingHealth + 0.5f);
        int presentHealth = Mathf.RoundToInt(health.PresentHealth + 0.5f);
        healthScore.text = presentHealth + "/" + maxHealth;
    }
}
