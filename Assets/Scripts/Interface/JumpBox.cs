using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JumpBox : TrainingMachineBase, IInteractable
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
            PlayerAnimation.instance.jumpboxTraining = true;
        }

        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.jumpboxTraining = true;
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            PlayerAnimation.instance.jumpboxTraining = false;
        }

        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.jumpboxTraining = false;
        }
    }


    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.jumpboxTraining += Time.deltaTime / 500;

            GameManager.instance.jumpboxTraining = Mathf.Clamp(GameManager.instance.jumpboxTraining, 0, 0.35f);
        }
    }
}
