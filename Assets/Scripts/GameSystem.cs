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

    readonly float attackFactor = 1;
    readonly float defenseFactor = 1;
    readonly float damageFactor = 20;
    readonly float armorDeflection = 10;
    readonly float speedFactor = 20;
    readonly float inititativeFactor = 20;


    private void Awake()
    {
        Instance = this;
    }

    public (int,float) TestAttack(float attack, float defense,float critModifier)
    {
        float hitProbability = CalculateHitChance(attack,defense);
        float highHitModifier = CalculateHighHitModifier(hitProbability);
        float randomNumber = Random.Range(0f, 1f);
        //Debug.Log("Random Number " + randomNumber+", hit probability "+hitProbability);
        if (randomNumber > hitProbability||randomNumber>maximumHitChance)
            return (0,highHitModifier);

        randomNumber = randomNumber / hitProbability;

        for (int i=0;i<SuccessLevelArray.Length;++i)
        {
            float critLevelCutoff = SuccessLevelArray[i] + (i+1) * critModifier;

            if (randomNumber < critLevelCutoff)
                return (SuccessLevelArray.Length - i,highHitModifier);
        }
        return (1,highHitModifier);
    }

    private float CalculateHighHitModifier(float hitProbability)
    {
        if (hitProbability < 1)
            return 1;
        else
            return
                hitProbability;
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
        return baseDamage *Mathf.Pow(2,power / damageFactor);
    }

    public float CalculateArmor(float armor, float damage)
    {
        if (armor <= 0)
            return damage;

        damage -= armor / armorDeflection;
          
        float randomNumber = Random.Range(0f, 100f);
        if (randomNumber < Mathf.Min(armor,95))
        {
            if (randomNumber > armor * 0.75f)
                return damage * 0.75f;
            if (randomNumber > armor * 0.5f)
                return damage / 2;
            if (randomNumber > armor / 4)
                return damage / 4;
            return 0;
        }
            

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
