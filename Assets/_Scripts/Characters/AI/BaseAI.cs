using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    protected CharacterStats stats;
    protected CharacterManager characterManager;

    protected void Awake()
    {
        stats = GetComponent<CharacterStats>();
        characterManager = FindObjectOfType<CharacterManager>();
    }

    public abstract IDamageable SelectTarget();
    public abstract PowerSO SelectPower();

}
