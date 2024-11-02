using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] mainCameraArray;
    [SerializeField] CinemachineVirtualCamera[] overheadCameraArray;
    [SerializeField] CameraTarget[] cameraTargetArray;


    //Uused varaibles for commented out stuff
    int activeCameraId;
    CinemachineTransposer cinemachineTransposer;
    Vector3 targetFollowOffset;

    float zoomYAmount = 2f;
    float zoomZAmount = 1f;
    private float minYFollowOffset = 1f;
    private float maxYFollowOffset = 20f;
    private float minZFollowOffset = 5f;
    private float maxZFollowOffset = 15f;
    private float zoomSpeed = 5f;
    float moveSpeed = 10f;
    float rotationSpeed = 80f;

    private void OnEnable()
    {
        LevelSetup.OnNewStage += SwitchCamera;
    }

    private void Start()
    {
        SwitchCamera(0);
    }

    private void OnDisable()
    {
        LevelSetup.OnNewStage -= SwitchCamera;
    }


    private void SwitchCamera(int cameraId)
    {
        if(cameraId>=mainCameraArray.Length)
        {
            Debug.LogWarning("Camera " + cameraId + "does not exist");
            return;
        }

        foreach (CinemachineVirtualCamera cvc in mainCameraArray)
            cvc.gameObject.SetActive(false);

        foreach (CinemachineVirtualCamera cvc in overheadCameraArray)
            cvc.gameObject.SetActive(false);

        foreach (CameraTarget ct in cameraTargetArray)
            ct.gameObject.SetActive(false);

        mainCameraArray[cameraId].gameObject.SetActive(true);
        overheadCameraArray[cameraId].gameObject.SetActive(true);
        cameraTargetArray[cameraId].gameObject.SetActive(true);

        activeCameraId = cameraId;
    }







    /*
    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }


    private void Update()
    {
        CameraMove();
        CameraRotate();
        CameraZoom();
    }
    */
    private void CameraMove()
    {
      /*  Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector(); 

        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;
      */
    }

    private void CameraRotate()
    {
        /*
        Vector3 inputRotation = new Vector3(0, 0, 0);
       
        inputRotation.y = InputManager.Instance.GetCameraRotateAmount();

        transform.eulerAngles += inputRotation * rotationSpeed * Time.deltaTime;
        */
    }

    private void CameraZoom()
    {
        /*
        float zoomIncreaseAmount = 0.05f;
        targetFollowOffset.y -= zoomIncreaseAmount * InputManager.Instance.GetCameraZoomAmount();
        
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, minYFollowOffset, maxYFollowOffset);

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
        */
        
    }
    /*
    public void FocusOnObject(Transform transform)
    {
       this.transform.position=transform.position;
       targetFollowOffset.y = 10;
    }
    */
}
