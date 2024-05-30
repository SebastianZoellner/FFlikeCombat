
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectInfoScreen : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Blurb;
    [SerializeField] TMP_Text AttributeModifiers;
    [SerializeField] TMP_Text Effects;
    [SerializeField] TMP_Text Health;
    [SerializeField] TMP_Text Defense;

    private ObjectStats stats;


    public void SetupScreen(Entity entity)
    {
        stats = entity.GetComponent<ObjectStats>();

        Name.text = stats.GetName();
        Icon.sprite = stats.GetIcon();
        if (stats.GetBlurb() != "")
            Blurb.text = stats.GetBlurb();

        SetAttributeModifiers();
    }

    private void SetAttributeModifiers()
    {
        string modifierText="";
        foreach(Faction f in Enum.GetValues(typeof(Faction)))
            foreach (Attribute a in Enum.GetValues(typeof(Attribute)))
            {
                if (stats.GetAttributeModifier(a, f) != 0)
                {
                    modifierText += a + " " + f + ": " + Mathf.RoundToInt(stats.GetAttributeModifier(a, f)) + "\n";
                }

            }


        AttributeModifiers.text = modifierText;

    }
    
        


}
