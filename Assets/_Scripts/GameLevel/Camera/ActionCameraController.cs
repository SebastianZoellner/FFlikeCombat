using UnityEngine;
using Cinemachine;

public class ActionCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetCameraFocus(Transform focus)
    {
        if (focus)
        {
            virtualCamera.Follow = focus;
            virtualCamera.LookAt = focus;
        }
    }
}
