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

        TrainingProgress();
    }


    public void DisplayMachineWarning()
    {
        if (GameManager.instance.bikeTraining == 0.35f)
        {
            IHM.instance.contextMessageCorout = StartCoroutine(MachineWarning());
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



    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 500;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);

            DisplayMachineWarning();
        }
    }
}
