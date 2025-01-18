using UnityEngine;

public class MovingSpots : MonoBehaviour
{
    [SerializeField] Transform spotBody;
    [SerializeField] float xMinAngle;
    [SerializeField] float xMaxAngle;
    [SerializeField] float yMinAngle;
    [SerializeField] float yMaxAngle;

    [SerializeField] float rotationSpeed = 0.05f;

    private float facingTreshold = 0.05f;
    private Quaternion targetAngle;
    private Quaternion startAngle;
    float rotationTime;
    


    private void Start()
    {
        SwitchDirection();
    }

    private void Update()
    {

        if (Quaternion.Angle(spotBody.rotation, targetAngle) < facingTreshold) {
            SwitchDirection();
            return;
        }

        rotationTime += Time.deltaTime;
        spotBody.rotation = Quaternion.Slerp(startAngle, targetAngle, rotationSpeed*rotationTime);

    }

     private void SwitchDirection()
    {
        startAngle = spotBody.rotation;
        rotationTime = 0;
        float newX = Random.Range(xMinAngle, xMaxAngle);
        float newY = Random.Range(yMinAngle, yMaxAngle);
        Vector3 newAngle = new Vector3(newX, newY, spotBody.rotation.z);
        targetAngle = Quaternion.Euler(newAngle);
    }


}
