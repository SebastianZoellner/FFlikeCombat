using UnityEngine;

public class EnemyController : MonoBehaviour
{
    IDamageable target;
    PowerSO selectedPower;
  
    CharacterInitiative initiative;
    BaseAI aiBrain;

    private void Awake()
    {
        initiative = GetComponent<CharacterInitiative>();
        aiBrain = GetComponent<BaseAI>();
    }
    
    public void SetNextAction()
    {
        target = aiBrain.SelectTarget();
        if (target==null)
            Debug.LogWarning("No target Selected");

        selectedPower = aiBrain.SelectPower();

        if (!selectedPower)
            Debug.LogWarning("No power Selected");
       
        //Debug.Log("Setting Attack " + selectedPower.buttonName + " against " + target.name);
        initiative.ReadyAttack(selectedPower,target);
    }
}
