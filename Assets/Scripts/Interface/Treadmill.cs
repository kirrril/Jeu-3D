using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Cinemachine;

public class Treadmill : TrainingMachineBase, IInteractable
{
    bool thisTreadmill;

    [SerializeField]
    CinemachineVirtualCamera playerCam;

    [SerializeField]
    Transform cameraTarget;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isJogging";
    }

    protected override void Update()
    {
        base.Update();

        if (thisTreadmill)
        {
            TreadmillTrainingProgress();
            DisplayMachineWarning();
            WaterManagement();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.treadmillTraining >= 0.35f)
        {
            if (IHM.instance.contextMessageCoroutName != "TreadmillTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(TreadmillTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "TreadmillTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator TreadmillTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "TREADMILL TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "TREADMILL TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "TREADMILL TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }


    void TreadmillTrainingProgress()
    {
        GameManager.instance.treadmillTraining += Time.deltaTime / 100;

        GameManager.instance.treadmillTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
    }

    void WaterManagement()
    {
        if (GameManager.instance.treadmillTraining < 0.35f)
        {
            float waterLoss = Time.deltaTime / 20;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0)
        {
            if (trainingAudio != null)
            {
                trainingAudio.Stop();
            }

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
            thisTreadmill = true;

            cameraTarget.localPosition = new Vector3(0f, 1.2f, 0f);
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(1.5f, 2f, -2f);
        }

        if (thisTreadmill && GameManager.instance.bikeTraining <= 0.35f)
        {
            IHM.instance.DisplayWaterWarning();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            thisTreadmill = false;

            cameraTarget.localPosition = new Vector3(0f, 1.4f, 0.64f);
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
        }
    }
}