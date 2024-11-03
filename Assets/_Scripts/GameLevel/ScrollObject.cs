using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    [SerializeField] Transform scrollPoint;

    [SerializeField] bool scrollX;
    [SerializeField] float scrollXdistance = 50f;
    [SerializeField] bool scrollY;
    [SerializeField] float scrollYdistance = 50f;
    [SerializeField] bool scrollZ;
    [SerializeField] float scrollZdistance = 50f;


    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3();
        newPosition = transform.position;
        bool scrollAction = false;

        if (scrollX == true && transform.position.x>scrollPoint.position.x)
        {
            newPosition.x = transform.position.x + scrollXdistance;
            scrollAction = true;
        }
        if (scrollY == true && transform.position.y > scrollPoint.position.y)
        {
            newPosition.y = transform.position.y + scrollYdistance;
            scrollAction = true;
        }

        if (scrollZ == true && transform.position.z > scrollPoint.position.z)
        {
            newPosition.z = transform.position.z + scrollZdistance;
            scrollAction = true;
        }

        if (scrollAction)
            transform.position = newPosition;
    }
}
