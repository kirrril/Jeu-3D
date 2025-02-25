using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Treadmill : TrainingMachineBase, IInteractable
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
            PlayerAnimation.instance.treadmillTraining = true;
        }

        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.treadmillTraining = true;
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            PlayerAnimation.instance.treadmillTraining = false;
        }

        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.treadmillTraining = false;
        }
    }


    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.treadmillTraining += Time.deltaTime / 500;

            GameManager.instance.treadmillTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }
}
