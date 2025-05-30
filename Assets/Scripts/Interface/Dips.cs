using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Dips : TrainingMachineBase, IInteractable
{
    bool thisDipStation;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isMakingDips";
    }

    protected override void Update()
    {
        base.Update();

        if (thisDipStation)
        {
            DipsTrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.dipsTraining >= 0.25f)
        {
            if (IHM.instance.contextMessageCoroutName != "DipsTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(DipsTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "DipsTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator DipsTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "DIPS TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "DIPS TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "DIPS TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }

    void WaterManagement()
    {
        if (GameManager.instance.dipsTraining < 0.25f)
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
        if (other.CompareTag("Girl"))
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);

            thisDipStation = true;

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(-0.5f, 1.6f, 0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(-1f, 2f, 1.5f);

            if (GameManager.instance.dipsTraining <= 0.25f)
            {
                IHM.instance.DisplayWaterWarning();
            }
        }
        else if (other.CompareTag("Man"))
        {
            base.OnTriggerEnter(other);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Man"))
        {
            base.OnTriggerExit(other);
        }
        else if (other.CompareTag("Player"))
        {
            base.OnTriggerExit(other);

            thisDipStation = false;

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
        }
    }


    void DipsTrainingProgress()
    {
        GameManager.instance.dipsTraining += Time.deltaTime / 100;

        GameManager.instance.dipsTraining = Mathf.Clamp(GameManager.instance.dipsTraining, 0, 0.25f);
    }
}
