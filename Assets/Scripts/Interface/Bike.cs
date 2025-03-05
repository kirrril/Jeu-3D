using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Bike : TrainingMachineBase, IInteractable
{
    void Start()
    {
        trainingDuration = 10.0f;

        animationBool = "isCycling";
    }

    protected override void Update()
    {
        base.Update();

        TrainingProgress();
    }


    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 500;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }
}
