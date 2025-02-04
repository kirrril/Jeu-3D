using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera playerCam;
    [SerializeField]
    private CinemachineVirtualCamera observerCam;


    void Start()
    {
        playerCam.Priority = 10;
        observerCam.Priority = 0;
    }

    void Update()
    {
        SetView();
    }

    void SetView()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            playerCam.Priority = 10;
            observerCam.Priority = 0;

        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            playerCam.Priority = 0;
            observerCam.Priority = 10;
        }
    }
}
