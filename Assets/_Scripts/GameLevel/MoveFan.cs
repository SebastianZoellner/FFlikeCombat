using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFan : MonoBehaviour
{
    [SerializeField] Transform fanbody;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    [SerializeField] float rotationSpeed = 0.05f;

    private float facingTreshold=0.05f;
    private Quaternion targetAngle;
    private bool increasing=false;


    private void Start()
    {
        SwitchDirection();
    }

    private void Update()
    {
        if (Quaternion.Angle(fanbody.rotation, targetAngle) < facingTreshold)
            SwitchDirection();
        fanbody.rotation = Quaternion.Slerp(fanbody.rotation, targetAngle, rotationSpeed);

    }

    private void SwitchDirection()
    {
       if(increasing)
        {
            fanbody.rotation = GetQuaternion(maxAngle);
            increasing = false;
            targetAngle = GetQuaternion(minAngle);
        }
        else
        {
            fanbody.rotation = GetQuaternion(minAngle);
            increasing = true;
            targetAngle = GetQuaternion(maxAngle);
        }
    }

    private Quaternion GetQuaternion(float angle)
    {
       return Quaternion.Euler(0, angle, 0);
    }
}
