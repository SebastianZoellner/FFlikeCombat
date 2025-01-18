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

    private Vector3 newPosition = new Vector3();

    // Update is called once per frame
    void Update()
    {
        newPosition = transform.position;
        bool scrollAction = false;

        if (scrollX == true && PastScrollPoint(transform.position.x,scrollPoint.position.x,scrollXdistance))
        {
            newPosition.x = transform.position.x + scrollXdistance;
            scrollAction = true;
        }
        if (scrollY == true && PastScrollPoint(transform.position.y,scrollPoint.position.y,scrollYdistance))
        {
            newPosition.y = transform.position.y + scrollYdistance;
            scrollAction = true;
        }

        if (scrollZ == true && PastScrollPoint(transform.position.z,scrollPoint.position.z,scrollZdistance))
        {
            newPosition.z = transform.position.z + scrollZdistance;
            scrollAction = true;
        }

        if (scrollAction)
            transform.position = newPosition;
    }

    private bool PastScrollPoint(float coordinate, float scrollPointCoordinate, float scrollDistance)
    {
        if ((coordinate > scrollPointCoordinate && scrollDistance < 0 )|| (coordinate < scrollPointCoordinate && scrollDistance > 0))
            return true;

        return false;
    }

}
