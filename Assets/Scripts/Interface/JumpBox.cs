using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class JumpBox : TrainingMachineBase, IInteractable
{
    bool thisJumpbox;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isBoxJumping";
    }

    protected override void Update()
    {
        base.Update();

        JumpboxTrainingProgress();

        DisplayMachineWarning();

        WaterManagement();
    }

    public void DisplayMachineWarning()
    {
        if (thisJumpbox && GameManager.instance.jumpboxTraining >= 0.35f)
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
        if (thisJumpbox)
        {
            GameManager.instance.jumpboxTraining += Time.deltaTime / 100;

            GameManager.instance.jumpboxTraining = Mathf.Clamp(GameManager.instance.jumpboxTraining, 0, 0.35f);
        }
    }

    void WaterManagement()
    {
        if (thisJumpbox && GameManager.instance.jumpboxTraining < 0.35f)
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

            StartCoroutine(ThirstyCorout());

            if (ambientSound != null) ambientSound.Play();

            GameManager.instance.currentPlayer.life -= 1;

            GameManager.instance.currentPlayer.water = 0.5f;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // if (trainingPerson == null)
        // {
            base.OnTriggerEnter(other);

            if (other.CompareTag("Player"))
            {
                thisJumpbox = true;
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
        }
    }
}