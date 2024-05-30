using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] float cameraMove=0.1f;
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
}
