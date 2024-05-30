
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIInfoScreen : MonoBehaviour
{

    [SerializeField] GameObject characterInfoScreen;
    [SerializeField] Image Icon;
    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text blurb;
    [SerializeField] TMP_Text attributes;
    [SerializeField] TMP_Text otherAttributes;
    [SerializeField] TMP_Text healthScore;
    [SerializeField] TMP_Text enduranceScore;
    [SerializeField] TMP_Text defense;
    [SerializeField] TMP_Text statuses;

    [SerializeField] GameObject objectInfoScreen;
    

    private CharacterHealth health;
    private StatusManager statusManager;
    private CharacterStats stats;

    private void OnEnable()
    {
        InputReader.OnSelectedEntityChanged += InputReader_OnSelectedEntityChanged;      
    }

    private void OnDisable()
    {
        InputReader.OnSelectedEntityChanged -= InputReader_OnSelectedEntityChanged;

        Unbind();
    }

    private void InputReader_OnSelectedEntityChanged(Entity entity)
    {
        if (!entity)
        {
            characterInfoScreen.SetActive(false);
            objectInfoScreen.SetActive(false);
            Unbind();
            return;
        }

        if (entity.type == EntityType.Character)
        {
            characterInfoScreen.SetActive(true);

            characterName.text = entity.Stats.GetName();
            Icon.sprite = entity.Stats.GetIcon();
            if (entity.Stats.GetBlurb() != "")
                blurb.text = entity.Stats.GetBlurb();
            health = entity.Health;
            statusManager = entity.StatusManager;
            stats = entity.Health.Stats;
            SetHealthParameters();
            SetAttributes();
            SetStatuses();
            Bind();
            return;
        }
        if (entity.type == EntityType.Object)
        {
            objectInfoScreen.SetActive(true);
            objectInfoScreen.GetComponent<UIObjectInfoScreen>().SetupScreen(entity);
        }
    }

    private void SetAttributes()
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

    private void SetStatuses()
    {
        string[] statusArray = statusManager.GetStatusNames();
       
        statuses.text = "";

        for (int i = 0; i < statusArray.Length; ++i)
        {
            statuses.text += statusArray[i] + "\n";
        }
    }

    private void SetHealthParameters()
    {
      
        healthScore.text = DisplayFloat(health.PresentHealth) + "/" + DisplayFloat(health.StartingHealth);
        enduranceScore.text = DisplayFloat(health.PresentEndurance) + "/" + DisplayFloat(health.StartingEndurance);
    }

    private void Bind()
    {
        health.OnHealthChanged += SetHealthParameters;
        statusManager.OnStatusChanged += StatusManager_OnStatusChanged;
        health.OnDied += Health_OnDied;
    }

    private void Health_OnDied()
    {
        characterInfoScreen.SetActive(false);
        objectInfoScreen.SetActive(false);
        Unbind();
    }

    private void StatusManager_OnStatusChanged()
    {
        SetAttributes();
        SetStatuses();
    }

    private void Unbind()
    {
        if (health)
        {
            health.OnHealthChanged -= SetHealthParameters;
            health.OnDied -= Health_OnDied;
        }
        if (statusManager)
            statusManager.OnStatusChanged -= StatusManager_OnStatusChanged;
    }

   

    
}
