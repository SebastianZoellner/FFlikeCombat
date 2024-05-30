using UnityEngine;

public class UIEnemyAvatar : MonoBehaviour
{
    SetAvatarDisplay setDisplay;

    private void Awake()
    {
        setDisplay = GetComponent<SetAvatarDisplay>();
    }

    private void OnEnable()
    {
        CharacterManager.OnEnemySelectedChanged += InputReader_OnEnemySelectedChanged;
    }

    private void OnDisable()
    {
        CharacterManager.OnEnemySelectedChanged -= InputReader_OnEnemySelectedChanged;
    }

    private void InputReader_OnEnemySelectedChanged(IDamageable health)
    {
        if(health is CharacterHealth)
            setDisplay.SetAvatar((CharacterHealth)health);
    }
}
