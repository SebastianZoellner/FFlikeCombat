using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class HoverTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public static event Action OnMouseLoseFocus;
    public static event Action<string, Vector2> OnMouseHover;
    private float timeToWait = 0.5f;



    private void OnDisable()
    {
        EndMouseOver();    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EndMouseOver();
    }

    protected void EndMouseOver()
    {
        StopAllCoroutines();
        OnMouseLoseFocus.Invoke();
    }

    private void ShowMessage()
    {
        OnMouseHover.Invoke(GetTip(), Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        ShowMessage();
    }

    protected abstract string GetTip();
   
}
