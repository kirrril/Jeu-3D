using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Cinemachine;

public class JumpBox : TrainingMachineBase, IInteractable
{
    bool thisJumpbox;

    [SerializeField]
    CinemachineVirtualCamera playerCam;

    [SerializeField]
    Transform cameraTarget;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isBoxJumping";
    }

    protected override void Update()
    {
        base.Update();

        if (thisJumpbox)
        {
            JumpboxTrainingProgress();
            DisplayMachineWarning();
            WaterManagement();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.jumpboxTraining >= 0.35f)
        {
            if (IHM.instance.contextMessageCoroutName != "JumpboxTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(JumpboxTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "JumpboxTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator JumpboxTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "JUMPBOX TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "JUMPBOX TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "JUMPBOX TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }


    void JumpboxTrainingProgress()
    {
        GameManager.instance.jumpboxTraining += Time.deltaTime / 100;

        GameManager.instance.jumpboxTraining = Mathf.Clamp(GameManager.instance.jumpboxTraining, 0, 0.35f);
    }

    void WaterManagement()
    {
        if (GameManager.instance.jumpboxTraining < 0.35f)
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
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            thisJumpbox = true;

            cameraTarget.localPosition = new Vector3(0f, 1.5f, 1.5f);
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, 2.5f);
        }

        if (thisJumpbox && GameManager.instance.bikeTraining <= 0.35f)
        {
            IHM.instance.DisplayWaterWarning();
        }
        // }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            thisJumpbox = false;

            cameraTarget.localPosition = new Vector3(0f, 1.4f, 0.64f);
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
        }
    }
}