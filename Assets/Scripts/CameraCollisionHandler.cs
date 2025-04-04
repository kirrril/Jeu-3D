using UnityEngine;
using Cinemachine;

public class CameraCollisionHandler : MonoBehaviour
{
    public Transform cameraProxy;
    public CinemachineVirtualCamera virtualCamera;

    void LateUpdate()
    {
        Vector3 idealPosition = virtualCamera.State.RawPosition;
        Vector3 proxyPosition = cameraProxy.position;

        if (Vector3.Distance(proxyPosition, idealPosition) > 0.1f)
        {
            virtualCamera.transform.position = proxyPosition;
        }
    }
}
