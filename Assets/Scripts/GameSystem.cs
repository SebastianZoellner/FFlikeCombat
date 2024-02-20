using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 
public class GameSystem : MonoBehaviour
{ 
    public static GameSystem Instance;

    [SerializeField] float baseHitChance = 0.8f;
    [SerializeField] float maximumHitChance = 0.95f;
    [SerializeField] float minimumHitChance = 0.1f;
    private float[] SuccessLevelArray = new float[] { 0.25f, 0.5f, 0.75f, 1 };

    readonly float attackFactor = 2;
    readonly float defenseFactor = 2;
    readonly float damageFactor = 10;
    readonly float armorDeflection = 10;
    readonly float speedFactor = 50;
    readonly float inititativeFactor = 50;


    private void Awake()
    {
        Instance = this;
    }

    public int TestAttack(float attack, float defense,float critModifier)
    {
        float hitProbability = CalculateHitChance(attack,defense);
        float randomNumber = Random.Range(0f, 1f);
        Debug.Log("Random Number " + randomNumber+", hit probability "+hitProbability);
        if (randomNumber > hitProbability||randomNumber>maximumHitChance)
            return 0;

        randomNumber = randomNumber / hitProbability;

        for (int i=0;i<SuccessLevelArray.Length;++i)
        {
            float critLevelCutoff = SuccessLevelArray[i] + (i+1) * critModifier;

            if (randomNumber < critLevelCutoff)
                return SuccessLevelArray.Length - i;
        }
        return 1;
    }

    public float CalculateAttack(float combat, float baseAttack)
    {
        return baseAttack + combat / attackFactor;
    }

    public float CalculateDefense(float agillity, float baseDefense) 
    {
        return baseDefense + agillity / defenseFactor;
    }

    public float CalculateDamage(float power, float baseDamage)
    {
        if (power > 0)
            return baseDamage * (1 + power / damageFactor);
        else
            return baseDamage / (1 - power / damageFactor);
    }

    public float CalculateArmor(float armor, float damage)
    {
        if (armor <= 0)
            return damage;

        damage -= armor / armorDeflection;
          
        float randomNumber = Random.Range(0f, 100f);
        if (randomNumber < Mathf.Min(armor,95))
            return damage / 2;

        return damage;
    }

    public float CalculateWaitTime(float speed,float baseTime)
    {
        if (speed > 0)
            return baseTime / (1 + speed / speedFactor);
        else
            return baseTime * (1 - speed / speedFactor);
    }

    public float CalculateReadytime(float initiative,float baseTime)
    {
        if (initiative > 0)
            return baseTime / (1 + initiative / inititativeFactor);
        else return baseTime * (1 - initiative / inititativeFactor);
    }

    private float CalculateHitChance(float attack,float defense)
    {
        float hitChance=baseHitChance+(attack-defense)/100;
        if (hitChance < minimumHitChance)
            hitChance = minimumHitChance;

        return hitChance;
    }


}
