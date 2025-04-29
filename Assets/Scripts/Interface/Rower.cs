using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rower : TrainingMachineBase, IInteractable
{
    bool thisRower;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPullingRower";
    }

    protected override void Update()
    {
        base.Update();

        if (thisRower)
        {
            RowerTrainingProgress();
            DisplayMachineWarning();
            WaterManagement();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.rowerTraining >= 0.167f)
        {
            if (IHM.instance.contextMessageCoroutName != "RowerTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(RowerTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "RowerTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator RowerTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "ROWER TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "ROWER TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "ROWER TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }

    void RowerTrainingProgress()
    {
        GameManager.instance.rowerTraining += Time.deltaTime / 100;

        GameManager.instance.rowerTraining = Mathf.Clamp(GameManager.instance.rowerTraining, 0, 0.167f);
    }

    void WaterManagement()
    {
        if (GameManager.instance.rowerTraining < 0.167f)
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
        if (!other.CompareTag("Player") && !other.CompareTag("Girl"))
        {
            return;
        }
        else
        {
            base.OnTriggerEnter(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("RowerIsMoving", true);

            if (other.CompareTag("Player"))
            {
                thisRower = true;

                if (GameManager.instance.rowerTraining <= 0.167f)
                {
                    IHM.instance.DisplayWaterWarning();
                }

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(2f, 1.3f, -0.7f);
            }
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
            base.OnTriggerExit(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("RowerIsMoving", false);

            if (other.CompareTag("Player"))
            {
                thisRower = false;

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
            }
        }
    }
}
