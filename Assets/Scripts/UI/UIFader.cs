using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{

    [SerializeField] private float fadeDuration = 5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup.alpha, 0));
    }

    public void FadeOut()
    {
        //Debug.Log("Fading out");
        StartCoroutine(FadeCanvasGroup(canvasGroup.alpha, 1));
    }

    private IEnumerator FadeCanvasGroup(float start, float end)
    {
        canvasGroup.blocksRaycasts = true;
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }
        
}
