using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Bike : TrainingMachineBase, IInteractable
{
    bool thisBike;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isCycling";
    }

    protected override void Update()
    {
        base.Update();

        BikeTrainingProgress();

        DisplayMachineWarning();

        WaterManagement();
    }


    public void DisplayMachineWarning()
    {
        if (thisBike && GameManager.instance.bikeTraining >= 0.35f)
        {
            if (IHM.instance.contextMessageCoroutName != "BikeTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(BikeTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "BikeTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    IEnumerator BikeTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "BIKE TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "BIKE TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "BIKE TRAINING COMPLETED";

        yield return new WaitForSeconds(0.5f);

        IHM.instance.contextMessage.text = "";
    }

    void BikeTrainingProgress()
    {
        if (thisBike)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 100;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.bikeTraining, 0, 0.35f);
        }
    }

    void WaterManagement()
    {
        if (thisBike && GameManager.instance.bikeTraining < 0.35f)
        {
            float waterLoss = Time.deltaTime / 20;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0)
        {
            trainingAudio.Stop();

            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            StartCoroutine(ThirstyCorout());

            ambientSound.Play();

            GameManager.instance.currentPlayer.life -= 1;

            GameManager.instance.currentPlayer.water = 0.5f;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            thisBike = true;
        }

        if (thisBike && GameManager.instance.bikeTraining <= 0.35f)
        {
            IHM.instance.DisplayWaterWarning();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            thisBike = false;
        }
    }
}
