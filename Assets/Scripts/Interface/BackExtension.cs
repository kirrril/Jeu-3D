using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BackExtension : TrainingMachineBase, IInteractable
{
    bool thisBackExtension;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isExtensingBack";
    }

    protected override void Update()
    {
        base.Update();

        if (thisBackExtension)
        {
            BackExtensionTrainingProgress();
            DisplayMachineWarning();
            WaterManagement();
        }
    }


    public void DisplayMachineWarning()
    {
        if (GameManager.instance.extensionTraining >= 0.167f)
        {
            if (IHM.instance.contextMessageCoroutName != "BackExtensionTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(BackExtensionTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "BackExtensionTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator BackExtensionTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "BACK EXTENSION TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "BACK EXTENSION TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "BACK EXTENSION TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }

    void BackExtensionTrainingProgress()
    {
        GameManager.instance.extensionTraining += Time.deltaTime / 100;

        GameManager.instance.extensionTraining = Mathf.Clamp(GameManager.instance.extensionTraining, 0, 0.167f);
    }

    void WaterManagement()
    {
        if (GameManager.instance.extensionTraining < 0.167f)
        {
            float waterLoss = Time.deltaTime / 20;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0)
        {
            trainingAudio.Stop();

            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            if (thirstyCoroutine == null)
            {
                thirstyCoroutine = StartCoroutine(ThirstyCorout());
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Girl"))
        {
            return;
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                thisBackExtension = true;

                if (GameManager.instance.extensionTraining <= 0.167f)
                {
                    IHM.instance.DisplayWaterWarning();
                }

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 0.9f, 0.5f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(-1f, 1.4f, 1.2f);
            }

            base.OnTriggerEnter(other);
        }

    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Girl"))
        {
            return;
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                thisBackExtension = false;

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
            }

            base.OnTriggerExit(other);
        }
    }
}
