using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    // public static CameraSwitch instance;

    [SerializeField]
    private CinemachineVirtualCamera playerCam;
    [SerializeField]
    private CinemachineVirtualCamera observerCam;


    // void Awake()
    // {
    //     if (instance != null && instance != this)
	// 	{
	// 		Destroy(this.gameObject);
	// 	}
	// 	else
	// 	{
	// 		instance = this;
	// 	}
    // }

    void Start()
    {
        // DontDestroyOnLoad(this.gameObject);
        
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
