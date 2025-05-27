using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Pole : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    Transform cameraTarget;

    [SerializeField]
    CinemachineVirtualCamera playerCam;

    [SerializeField]
    Transform cameraPlace;


    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Switch());
    }

    void OnTriggerExit(Collider other)
    {
        StartCoroutine(SwitchBack());
    }

    IEnumerator Switch()
    {
        yield return new WaitForSeconds(0.3f);
        cameraTarget.localPosition = new Vector3(0f, 1.6f, 0f);
        playerCam.Follow = cameraPlace;
        CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        playerTransposer.m_FollowOffset = new Vector3(2f, 0.5f, -2f);
    }

    IEnumerator SwitchBack()
    {
        yield return new WaitForSeconds(0.1f);
        cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);
        playerCam.Follow = player;
        CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
    }
}
