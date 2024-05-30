using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [SerializeField] private SimpleAudioEventSO clickSound;
    [SerializeField] private float actionDelay = 0.15f;
    [SerializeField] private Button button;
    [SerializeField] private float scaleFactor=0.8f;

    [SerializeField] private UnityEvent OnButtonPressed;
    private Vector3 originalScale;


    protected void Start()
    {
        if (!button)
            button = GetComponent<Button>();
        originalScale = button.transform.localScale;
    }

    public void ActivateButton()
    {
        //Debug.Log("In Activate Button", gameObject);
        if(clickSound)
        clickSound.Play(AudioManager.UIFXSource);
        StartCoroutine(ActionAfterDelay());
    }

    private IEnumerator ActionAfterDelay()
    {
        float timer = 0;
        while(timer<actionDelay)
        {
            float t = timer / actionDelay;
            Vector3 currentScale = Vector3.Lerp(originalScale, originalScale * scaleFactor, t);
            button.transform.localScale = currentScale;
            timer += Time.deltaTime;
            yield return null;
        }
        
        ButtonAction();
        OnButtonPressed.Invoke();

        timer = 0;
        while (timer < actionDelay)
        {
            float t = timer / actionDelay;
            Vector3 currentScale = Vector3.Lerp(originalScale*scaleFactor, originalScale, t);
            button.transform.localScale = currentScale;
            timer += Time.deltaTime;
            yield return new WaitForSecondsRealtime(0);
        }
        button.transform.localScale = originalScale;
    }

    protected abstract void ButtonAction();

}

