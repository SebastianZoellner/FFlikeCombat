using UnityEngine;

public class PowerTest : MonoBehaviour
{

    [SerializeField] CharacterCombat testCombat;
    [SerializeField] PowerSO testPower;
    [SerializeField] bool targetObject;
    [SerializeField] CharacterHealth target;
    [SerializeField] ObjectHealth objectHealth;


    private void Start()
    {if(SpawnPointController.Instance)
        SpawnPointController.Instance.SetupStage(1);
    }


    public void OnButtonPressed()
    {
        if (!testCombat)
            Debug.LogError("No character Set");
        if (!testPower)
            Debug.Log("No power set");
        if (targetObject)
            testCombat.StartAttack(testPower, objectHealth);
        else
            testCombat.StartAttack(testPower, target);
    }
}
