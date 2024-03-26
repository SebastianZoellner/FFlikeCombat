
using UnityEngine;

public class UIPlayerAvatar : MonoBehaviour
{
    SetAvatarDisplay setDisplay;

    private void Awake()
    {
        setDisplay = GetComponent<SetAvatarDisplay>();
    }

    private void OnEnable()
    {
        CharacterManager.OnPlayerSelectedChanged += InputReader_OnPlayerSelectedChanged;
    }

    private void OnDisable()
    {
        CharacterManager.OnPlayerSelectedChanged -= InputReader_OnPlayerSelectedChanged;
    }

    private void InputReader_OnPlayerSelectedChanged(CharacterHealth health)
    {     
        setDisplay.SetAvatar(health);       
    }
}
