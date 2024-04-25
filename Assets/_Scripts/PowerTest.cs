using UnityEngine;

public class PowerTest : MonoBehaviour
{

    [SerializeField] CharacterCombat testCombat;
    [SerializeField] PowerSO testPower;
    [SerializeField] CharacterHealth target;

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

        testCombat.StartAttack(testPower, target);
    }
}
