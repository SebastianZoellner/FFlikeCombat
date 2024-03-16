using System;
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

        health.OnHealthChanged += ChangeHealthBar;
        display.SetActive(true);

        playerName.text = health.Stats.GetName();
        avatar.sprite = health.Stats.GetIcon();
        level.text = Mathf.RoundToInt(health.Stats.GetLevel()).ToString();
        ChangeHealthBar();
    }

    private void ChangeHealthBar()
    {
        healthBar.fillAmount = health.PresentHealth / health.StartingHealth;
    } 
    
    private void Deregister()
    {
        if (health)
            health.OnHealthChanged -= ChangeHealthBar;
    }
}
