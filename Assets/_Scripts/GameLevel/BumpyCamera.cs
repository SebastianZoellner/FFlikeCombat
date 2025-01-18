using UnityEngine;

public class BumpyCamera : MonoBehaviour
{
    [SerializeField] private  float bumpIntensity = 0.5f; // How strong the bumps are
    [SerializeField] private float bumpFrequency = 10f; // How often bumps occur
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float bumpX = Mathf.PerlinNoise(Time.time * bumpFrequency, 0) * bumpIntensity;
        float bumpY = Mathf.PerlinNoise(0, Time.time * bumpFrequency) * bumpIntensity;

        Vector3 bumpOffset = new Vector3(bumpX, bumpY, 0);
        transform.localPosition = initialPosition + bumpOffset;
    }
}
