using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetAvatarDisplay : MonoBehaviour
{
    [SerializeField] GameObject display;
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text level;
    [SerializeField] Image healthBar;
    [SerializeField] Image experienceBar;
    [SerializeField] Image enduranceBar;
    [SerializeField] Image avatar;

    private CharacterHealth health;
    private CharacterExperience experience;

    private void OnDisable()
    {
        Deregister(); 
    }

    public void SetAvatar(CharacterHealth health)
    {
        Deregister();

        if (!health)
        {
            display.SetActive(false);
            return;
        }

        this.health = health;
        experience = health.GetComponent<CharacterExperience>();
        health.OnHealthChanged += ChangeHealthBar;
        if(experience)
        experience.OnExperienceChanged += ChangeExpereinceBar;
        display.SetActive(true);

        playerName.text = health.Stats.GetName();
        avatar.sprite = health.Stats.GetIcon();
        level.text = Mathf.RoundToInt(health.Stats.GetRank()).ToString();
        ChangeHealthBar();
        ChangeExpereinceBar();
    }

 public void HideAvatar()
    {
        Deregister();
        display.SetActive(false);
    }

    private void ChangeHealthBar()
    {
        healthBar.fillAmount = health.PresentHealth / health.StartingHealth;
        enduranceBar.fillAmount = health.PresentEndurance / health.StartingEndurance;
    } 

    private void ChangeExpereinceBar()
    {
        if (!experience)
            return;
        experienceBar.fillAmount = experience.experience / experience.GetLevelUpCost();
    }

    private void Deregister()
    {
        if (health)
            health.OnHealthChanged -= ChangeHealthBar;
        if (experience)
            experience.OnExperienceChanged -= ChangeExpereinceBar;
    }
}
