using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbell : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPushingBarbell";
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
        animator.SetBool("barbellIsMoving", true);
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("barbellIsMoving", false);
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