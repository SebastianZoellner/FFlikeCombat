using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTest : MonoBehaviour
{

    [SerializeField] PCController testHero;
    [SerializeField] PowerSO testPower;
    [SerializeField] CharacterHealth target;

    private CharacterCombat combat;

    private void Awake()
    {
        combat = testHero.GetComponent<CharacterCombat>();
    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void OnButtonPressed()
    {
        combat.StartAttack(testPower, target);
    }
}
