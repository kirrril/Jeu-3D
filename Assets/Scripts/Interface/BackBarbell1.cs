using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BackBarbell1 : TrainingMachineBase, IInteractable
{
    bool thisBackBarbell1;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPullingBackBarbell1";

        machineAnimationBool = "backBarbell1IsPulled";
    }

    protected override void Update()
    {
        base.Update();

        if (thisBackBarbell1)
        {
            BackBarbell1TrainingProgress();
            DisplayMachineWarning();
            WaterManagement();
        }
    }


    public void DisplayMachineWarning()
    {
        if (GameManager.instance.backBarbell1Training >= 0.167f)
        {
            if (IHM.instance.contextMessageCoroutName != "BackBarbell1TrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(BackBarbell1TrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "BackBarbell1TrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator BackBarbell1TrainingCompletedWarning()
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

    void BackBarbell1TrainingProgress()
    {
        GameManager.instance.backBarbell1Training += Time.deltaTime / 100;

        GameManager.instance.backBarbell1Training = Mathf.Clamp(GameManager.instance.backBarbell1Training, 0, 0.167f);
    }

    void WaterManagement()
    {
        if (GameManager.instance.backBarbell1Training < 0.167f)
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
        if (!other.CompareTag("Player") && !other.CompareTag("Man"))
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            thisBackBarbell1 = true;

            base.OnTriggerEnter(other);

            // Animator animator = GetComponentInChildren<Animator>();
            // animator.SetBool(machineAnimationBool, true);

            if (GameManager.instance.backBarbell1Training <= 0.167f)
            {
                IHM.instance.DisplayWaterWarning();
            }

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.5f, 0f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(-1.5f, 1.2f, 0.7f);
        }
        else
        {
            base.OnTriggerEnter(other);

            // Animator animator = GetComponentInChildren<Animator>();
            // animator.SetBool("BackBarbell1IsPulled", true);
        }

    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Man"))
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            thisBackBarbell1 = false;

            base.OnTriggerExit(other);

            // Animator animator = GetComponentInChildren<Animator>();
            // animator.SetBool("BackBarbell1IsPulled", false);

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
        }
        // else
        // {
        //     base.OnTriggerExit(other);

        //     Animator animator = GetComponentInChildren<Animator>();
        //     animator.SetBool("BackBarbell1IsPulled", false);
        // }
    }
}
