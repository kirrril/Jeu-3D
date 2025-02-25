using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bike : TrainingMachineBase, IInteractable
{
    protected override void Update()
    {
        base.Update();

        TrainingProgress();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            PlayerAnimation.instance.bikeTraining = true;
        }

        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.bikeTraining = true;
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            PlayerAnimation.instance.bikeTraining = false;
        }

        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.bikeTraining = false;
        }
    }



    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 500;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.bikeTraining, 0, 0.35f);
        }
    }
}
