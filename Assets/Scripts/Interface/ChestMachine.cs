using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChestMachine : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isTrainingChest_1";
    }

    protected override void Update()
    {
        base.Update();

        Chest1TrainingProgress();
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.chest1Training >= 0.35f)
        {
            IHM.instance.contextMessageCorout = StartCoroutine(MachineWarning());

            trainingAudio.Stop();
        }
    }

    public IEnumerator MachineWarning()
    {
        IHM.instance.contextMessage.text = $"TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("chestMachineIsMoving", true);

        if (other.CompareTag("Player"))
        {
            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.5f, 1.5f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("chestMachineIsMoving", false);

         if (other.CompareTag("Player"))
            {
                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);

                CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
                observerTransposer.m_FollowOffset = new Vector3(4f, 4.5f, -2f);
            }
    }


    void Chest1TrainingProgress()
    {
        if (PlayerController.instance.isTraining && trainingPerson == PlayerController.instance.gameObject && GameManager.instance.bikeTraining < 0.35f)
        {
            GameManager.instance.chest1Training += Time.deltaTime / 500;

            GameManager.instance.chest1Training = Mathf.Clamp(GameManager.instance.chest1Training, 0, 0.35f);
        }
    }
}
