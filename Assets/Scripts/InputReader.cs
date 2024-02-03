using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static event Action<Entity> OnSelectedEntityChanged = delegate { };
    
    public static Entity SelectedEntity { get; private set; }

    public event Action<int> OnAttackSelected = delegate { };
    public event Action<CharacterHealth> OnCharacterSelected = delegate { };
    public event Action OnDeselected = delegate { };

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

    //----------------------------------------------
    //       public functions
    //----------------------------------------------

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        PowerButton(1);
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        PowerButton(2);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        //We will want to check if action is ongoing 

        CharacterHealth targetHealth = GetTarget<CharacterHealth>();

        if (targetHealth)
        {
            OnCharacterSelected.Invoke(targetHealth);
        }
        else
            OnCharacterSelected.Invoke(null);     
    }

    public void OnExplore(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Entity entity = GetTarget<Entity>();

        if (entity)
        {
            OnSelectedEntityChanged.Invoke(entity);
            SelectedEntity = entity;
        }
        else
        {
            OnSelectedEntityChanged.Invoke(null);
            SelectedEntity = null;
        }
    }

    public void OnDeselect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnDeselected.Invoke();
        OnSelectedEntityChanged.Invoke(null);
        SelectedEntity = null;
    }
    //---------------------------------------------
    //     private functions
    //---------------------------------------------

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
        
        OnAttackSelected.Invoke(powerId);
        StartCoroutine(DisableKeyboard());
    }

    private Vector2 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    
    private T GetTarget<T>()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetMouseScreenPosition());
        T target;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, SelectableLayerMask))
        {
            target = raycastHit.collider.GetComponentInParent<T>();
            //Debug.Log("Ray hit " +raycastHit.collider.name+ "Component "+targetHealth.name);
            return target;
        }
        else
        {
            return default(T);
        }
    }


}