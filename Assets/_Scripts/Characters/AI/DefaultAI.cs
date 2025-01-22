using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAI : BaseAI
{
    private CharacterHealth lastTarget;

    public override PowerSO SelectPower()
    {
        return SelectStrongestPower();
    }

    public override IDamageable SelectTarget()
    {
        return SelectRandomTarget();
    }

   private CharacterHealth SelectRandomTarget()
    {
        int selectedIndex = Random.Range(0, characterManager.heroList.Count);
        //Debug.Log("Target selected " + selectedIndex);
        return characterManager.heroList[selectedIndex].GetComponent<CharacterHealth>();
    }

    private CharacterHealth SelectWeakestTarget()
    {
        CharacterHealth target = lastTarget;

        foreach (PCController pcc in characterManager.heroList)
        {
            //Debug.Log("Evaluating " + pcc.GetName());
            CharacterHealth health = pcc.GetComponent<CharacterHealth>();
            if (!target || target.PresentHealth > health.PresentHealth)
                target = health;
        }
        return target;
    }

    public PowerSO SelectStrongestPower()
    {
        PowerSO[] powerList = stats.GetAvailablePowers(false);

        //Debug.Log(powerList.Length + " powers available");

        //Find power with largest endurance cost
        PowerSO returnedPower = powerList[0];
        for (int i = 1; i < powerList.Length; ++i)
        {
            if (powerList[i].GetEnduranceCost() > returnedPower.GetEnduranceCost())
                returnedPower = powerList[i];
        }
        return returnedPower;
    }
}
