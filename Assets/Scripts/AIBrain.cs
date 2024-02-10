using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public static AIBrain Instance;
    [SerializeField] CharacterManager characterManager;
    CharacterHealth lastTarget;

    private void Awake()
    {
        Instance = this;
    }


    public CharacterHealth SelectWeakestTarget()
    {
        CharacterHealth target = lastTarget;

        foreach (PCController pcc in characterManager.playerCharacterList)
        {
            //Debug.Log("Evaluating " + pcc.GetName());
            CharacterHealth health = pcc.GetComponent<CharacterHealth>();
            if (!target || target.PresentHealth > health.PresentHealth)
                target = health;
        }
        return target;
    }

    public PowerSO SelectPower(CharacterStats stats)
    {
        int selectedIndex=Random.Range(0,stats.GetNumbeOfPowers());
        return stats.GetPower(selectedIndex);
    }

}
