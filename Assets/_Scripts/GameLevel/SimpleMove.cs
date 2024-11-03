using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] bool moveX;
    [SerializeField] float moveXspeed = 2f;
    [SerializeField] bool moveY;
    [SerializeField] float moveYspeed = 2f;
    [SerializeField] bool moveZ;
    [SerializeField] float moveZspeed = 2f;

    
    private void Update()
    {
        Vector3 move = new Vector3();

        if (moveX == true)
        {
            move.x = Time.deltaTime * moveXspeed;
        }
        if (moveY == true)
        {
            move.y = Time.deltaTime * moveYspeed;
        }

        if (moveZ == true)
        {
            move.z = Time.deltaTime * moveZspeed;
        }

        transform.position += move;
    }
}
