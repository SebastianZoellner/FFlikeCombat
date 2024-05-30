using System.Collections;
using UnityEngine;

public class SimpleFlickerLight : MonoBehaviour
{
    [SerializeField] Light lightAsset;
    [SerializeField] GameObject solidLightObject;

    [SerializeField] bool smoothFlicker;
    //[SerializeField] bool onOffFlicker;

[Header("For smooth flicker")]
    // min max intensities
    [SerializeField] float minIntensity = 1.25f;
    [SerializeField] float maxIntensity = 2f;

    [SerializeField] float flickerSpeed = 3.75f; // Speed of the flickering effect.
    [SerializeField] float smoothingFactor = 9f; // Controls the smoothness of intensity changes.

    [Header("For instant flicker")]
    [SerializeField] float minOnTime = 2f;
    [SerializeField] float maxOnTime = 20f;
    [SerializeField] float minOffTime = 0.1f;
    [SerializeField] float maxOffTime = 0.7f;


    private Coroutine flickerCoroutine;
    private float targetIntensity;

    void Start()
    {
        //check if there is a light
        if (lightAsset == null)
        {
            //attempt to get light transform if none is defined
            lightAsset = GetComponent<Light>();
            {
                //disable script if nothing is found
                if (!lightAsset)
                {
                    Debug.LogError("No Light Source Detected, Disabling Script:  " + transform.name);
                    enabled = false;
                    return;
                }
            }
        }
        if (smoothFlicker)
            flickerCoroutine = StartCoroutine(ambientLight());
        else
            flickerCoroutine = StartCoroutine(OnOffLight());

    }


    //stops coroutine if disabled in scene
    void OnDisable()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }
    }

    IEnumerator ambientLight()
    {
        while (true)
        {
            //random initial target intensity
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            float elapsedTime = 0f;
            float startIntensity = lightAsset.intensity;

            while (elapsedTime < 1f)
            {
                lightAsset.intensity = Mathf.SmoothStep(startIntensity, targetIntensity, elapsedTime);
                elapsedTime += Time.deltaTime * smoothingFactor;
                yield return null;
            }

            //time to wait before next flicker target
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f) / flickerSpeed);
        }
    }

    IEnumerator OnOffLight()
    {
        float onIntensity = 1;
        bool isOn = false;
        if (lightAsset.intensity > 0)
        {
            isOn = true;
            onIntensity = lightAsset.intensity;
        }

        while (true)
        {
            float elapsedTime = 0f;
            float waitTime;
            if (isOn)
                waitTime = Random.Range(minOnTime, maxOnTime);
            else
                waitTime = Random.Range(minOffTime, maxOffTime);

            while (elapsedTime < waitTime)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            if (isOn)
            {
                lightAsset.intensity = 0;
                if (solidLightObject)
                    solidLightObject.SetActive(false);
                isOn = false;
            }
            else
            {
                lightAsset.intensity = onIntensity;
                if (solidLightObject)
                    solidLightObject.SetActive(true);
                isOn = true;
            }

        }
    }

}