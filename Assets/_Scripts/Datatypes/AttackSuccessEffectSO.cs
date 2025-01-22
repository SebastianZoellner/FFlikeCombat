using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Effect", menuName = "Game Elements/Powers/Attack Effect")]

public class AttackSuccessEffectSO : ScriptableObject
{
    public StatusName status;
    public float intensity;
    public int duration;
    public float damageCost;
}
