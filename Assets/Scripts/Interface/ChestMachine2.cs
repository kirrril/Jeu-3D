using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChestMachine2 : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isTrainingChest_2";
    }

    protected override void Update()
    {
        base.Update();

        Chest2TrainingProgress();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl"))
        {
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);
        }
        else if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachine2IsMoving", true);

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.8f, 1.8f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);
        }
        else if (other.CompareTag("Man"))
        {
            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachine2IsMoving", true);

            base.OnTriggerEnter(other);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Man"))
        {
            base.OnTriggerExit(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachine2IsMoving", false);
        }
        else if (other.CompareTag("Player"))
        {
            base.OnTriggerExit(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachine2IsMoving", false);

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(4f, 4.5f, -2f);
        }
        else
        {
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(false);
        }
    }


    void Chest2TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.chest2Training += Time.deltaTime / 500;

            GameManager.instance.chest2Training = Mathf.Clamp(GameManager.instance.chest2Training, 0, 0.35f);
        }
    }
}
