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

    
    private void Awake()
    {
        Instance = this;
    }

    public int TestAttack(float attack, float defense,float critModifier)
    {
        float hitProbability = CalculateHitChance(attack,defense);
        float randomNumber = Random.Range(0f, 1f);
        Debug.Log("Random Number " + randomNumber);
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

    private float CalculateHitChance(float attack,float defense)
    {
        float hitChance=baseHitChance+(attack-defense)/100;
        if (hitChance < minimumHitChance)
            hitChance = minimumHitChance;

        return hitChance;
    }
}
