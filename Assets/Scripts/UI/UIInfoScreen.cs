
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIInfoScreen : MonoBehaviour
{

    [SerializeField] GameObject infoScreen;
    [SerializeField] Image Icon;
    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text blurb;
    [SerializeField] TMP_Text attributes;
    [SerializeField] TMP_Text otherAttributes;
    [SerializeField] TMP_Text healthScore;
    [SerializeField] TMP_Text defense;
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
        Icon.sprite = entity.Stats.GetIcon();
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

        defense.text = DisplayFloat( stats.GetDefenseValue());

    }

    private string DisplayFloat(float v)
    {
        return (Mathf.RoundToInt(v + 0.5f)).ToString();
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
        
        healthScore.text = DisplayFloat(health.PresentHealth) + "/" + DisplayFloat(health.StartingHealth);
    }
}
