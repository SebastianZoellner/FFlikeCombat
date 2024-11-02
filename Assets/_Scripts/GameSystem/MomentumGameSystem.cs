using UnityEngine;

public class MomentumGameSystem : MonoBehaviour
{
    private const float deathMultiplier = 4;
    private const float heavyHitMultiplier = 1;

    public float AttackMomentumChange(float change, int level)
    {
        if (level == 0)
            change = change * 0.5f;
        else
            change = change * level;

        return change;
    }

    public float HeavyHitMomentum(int targetLevel) => targetLevel * heavyHitMultiplier;

    public float DeathMomentum(int killedLevel) => killedLevel * deathMultiplier;
    
}
