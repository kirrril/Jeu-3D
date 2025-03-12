using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMachine : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isTrainingChest_1";
    }

    protected override void Update()
    {
        base.Update();

        TrainingProgress();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("chestMachineIsMoving", true);
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("chestMachineIsMoving", false);
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
