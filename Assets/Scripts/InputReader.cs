using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour,Controls.IPlayerActions
{
    public event Action <int> OnAttackSelected = delegate { };
    public event Action<CharacterHealth> OnTargetSelected = delegate { };

    [SerializeField] private LayerMask SelectableLayerMask;
    private Controls controls;
    private bool keysDisabled;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        PowerButton(1);
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        PowerButton(2);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        CharacterHealth targetHealth=GetTarget();
        if (targetHealth)
            OnTargetSelected.Invoke(targetHealth);
    }

    IEnumerator DisableKeyboard()
    {
        keysDisabled = true;
        yield return new WaitForSeconds(0.5f);
        keysDisabled = false;
    }

    

    private void PowerButton(int powerId)
    {
        if (keysDisabled)
            return;
       // Debug.Log("Attack Signal "+powerId);
        OnAttackSelected.Invoke(powerId);
        StartCoroutine(DisableKeyboard());
    }

    private Vector2 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    private CharacterHealth GetTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetMouseScreenPosition());
        CharacterHealth targetHealth;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, SelectableLayerMask))
        {
            targetHealth=raycastHit.collider.GetComponentInParent<CharacterHealth>();
            //Debug.Log("Ray hit " +raycastHit.collider.name+ "Component "+targetHealth.name);
            return targetHealth;
        }
        else
        {
            return null;
        }
    }
}
