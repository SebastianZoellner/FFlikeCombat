using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStats 
{
    public string GetName();
    public string GetBlurb();
    public Sprite GetIcon();
    public float GetStartingHealth();
    public int GetLevel();
    public float GetDefenseValue();
}
