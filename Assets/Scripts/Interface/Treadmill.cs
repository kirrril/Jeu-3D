using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Treadmill : TrainingMachineBase, IInteractable
{
    bool thisTreadmill;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isJogging";
    }

    protected override void Update()
    {
        base.Update();

        TreadmillTrainingProgress();

        DisplayMachineWarning();

        WaterManagement();
    }

    public void DisplayMachineWarning()
    {
        if (thisTreadmill && GameManager.instance.treadmillTraining >= 0.35f)
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
        if (thisTreadmill)
        {
            GameManager.instance.treadmillTraining += Time.deltaTime / 100;

            GameManager.instance.treadmillTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }

    void WaterManagement()
    {
        if (thisTreadmill && GameManager.instance.treadmillTraining < 0.35f)
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
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            thisTreadmill = true;
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
        }
    }
}