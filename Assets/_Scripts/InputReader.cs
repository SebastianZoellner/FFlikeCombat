using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static event Action<Entity> OnSelectedEntityChanged = delegate { };
    
    public static Entity SelectedEntity { get; private set; }
    public event Action OnNextEnemy = delegate { };
    public event Action<int> OnAttackSelected = delegate { };
    public static event Action<IDamageable> OnCharacterSelected = delegate { };
    public static event Action OnDeselected = delegate { };

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

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Mouse is over UI element!");
            return;
        }
        //We will want to check if action is ongoing 

       // Debug.Log("Select Button Down");

        IDamageable targetHealth = GetTarget<IDamageable>();

        if (targetHealth!=null)
        {
            OnCharacterSelected.Invoke(targetHealth);
        }
        else
            OnCharacterSelected.Invoke(null);     
    }

    public void OnExplore(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Mouse is over UI element!");
            return;
        }

        //Debug.Log("Explore button down");
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

        //Debug.Log("Get Target Called");
        Ray ray = Camera.main.ScreenPointToRay(GetMouseScreenPosition());
        T target;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, SelectableLayerMask))
        {
            target = raycastHit.collider.GetComponentInParent<T>();
            //Debug.Log("Ray hit " +raycastHit.collider.name+ "Component ");
            return target;
        }
        else
        {
            return default(T);
        }
    }

    public void OnNextTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnNextEnemy.Invoke();
    }
}