using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Bike : TrainingMachineBase, IInteractable
{
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
    }


    public void DisplayMachineWarning()
    {
        if (GameManager.instance.bikeTraining >= 0.35f && trainingPerson == PlayerController.instance.gameObject)
        {
            IHM.instance.contextMessageCorout = StartCoroutine(MachineWarning());

            trainingAudio.Stop();
        }
    }

    public IEnumerator MachineWarning()
    {
        IHM.instance.contextMessage.text = $"BIKE TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"BIKE TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"BIKE TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    void BikeTrainingProgress()
    {
        if (PlayerController.instance.isTraining && trainingPerson == PlayerController.instance.gameObject && GameManager.instance.bikeTraining < 0.35f)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 500;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.bikeTraining, 0, 0.35f);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
