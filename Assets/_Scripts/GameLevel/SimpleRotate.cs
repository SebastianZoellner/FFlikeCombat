using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] bool rotX;
    [SerializeField] float rotXSpeed = 50f;
    [SerializeField] bool rotY;
    [SerializeField] float rotYSpeed = 50f;
    [SerializeField] bool rotZ;
    [SerializeField] float rotZSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        if (rotX == true)
        {
            transform.Rotate(Vector3.left * Time.deltaTime * rotXSpeed);
        }
        if (rotY == true)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotYSpeed);
        }

        if (rotZ == true)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * rotZSpeed);
        }

    }


}
