using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Barbell : TrainingMachineBase, IInteractable
{
    bool thisBarbell;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPushingBarbell";
    }

    protected override void Update()
    {
        base.Update();

        if (thisBarbell)
        {
            BarbellTrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.barbellTraining >= 0.25f)
        {
            if (IHM.instance.contextMessageCoroutName != "BarbellTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(BarbellTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "BarbellTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator BarbellTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "BARBELL TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "BARBELL TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "BARBELL TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }

    void WaterManagement()
    {
        if (GameManager.instance.barbellTraining < 0.25f)
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

            // ambientSound.Play();

            // GameManager.instance.currentPlayer.life -= 1;

            // GameManager.instance.currentPlayer.water = 0.5f;
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

            thisBarbell = true;

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellisMoving", true);

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.5f, 0.8f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);

            if (GameManager.instance.barbellTraining <= 0.25f)
            {
                IHM.instance.DisplayWaterWarning();
            }
        }
        else if (other.CompareTag("Man"))
        {
            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellisMoving", true);

            base.OnTriggerEnter(other);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Man"))
        {
            base.OnTriggerExit(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellisMoving", false);
        }
        else if (other.CompareTag("Player"))
        {
            base.OnTriggerExit(other);

            thisBarbell = false;

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellisMoving", false);

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


    void BarbellTrainingProgress()
    {
        GameManager.instance.barbellTraining += Time.deltaTime / 100;

        GameManager.instance.barbellTraining = Mathf.Clamp(GameManager.instance.barbellTraining, 0, 0.25f);
    }
}