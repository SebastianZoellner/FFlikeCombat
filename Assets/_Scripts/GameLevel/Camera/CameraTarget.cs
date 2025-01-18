using System;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] float cameraMove=0.1f;

    [SerializeField] private bool bumpyCamera;
    [SerializeField] private float bumpXIntensity = 0.1f; // How strong the bumps are
    [SerializeField] private float bumpYIntensity = 0.1f; // How strong the bumps are
    [SerializeField] private float bumpFrequency = 15f; // How often bumps occur

    private Transform activeCharacter;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        ActionSequencer.OnNextActorStarting += ActionSequencer_OnNextActorStarting;
        ActionSequencer.OnNoActor += ActionSequencer_OnNoActor;
    }

    private void Update()
    {
        if (activeCharacter)
            transform.position = startPosition+cameraMove*(activeCharacter.position-startPosition);
        else
            transform.position = startPosition;

        AddBumpyCamera();
    }

    

    private void OnDisable()
    {
        ActionSequencer.OnNextActorStarting -= ActionSequencer_OnNextActorStarting;
        ActionSequencer.OnNoActor -= ActionSequencer_OnNoActor;
    }

    private void ActionSequencer_OnNextActorStarting(CharacterInitiative character)
    {
        activeCharacter = character.transform;
    }

    private void ActionSequencer_OnNoActor()
    {
        activeCharacter = null;
    }
    
    private void AddBumpyCamera()
    {
        if (!bumpyCamera)
            return;

        float bumpX = Mathf.PerlinNoise(Time.time * bumpFrequency, 0) * bumpXIntensity;
        float bumpY = Mathf.PerlinNoise(0, Time.time * bumpFrequency) * bumpYIntensity;

        Vector3 bumpOffset = new Vector3(bumpX, bumpY, 0);

        transform.localPosition += bumpOffset;
    }
}
