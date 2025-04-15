using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squatting : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isSquatting";
    }

    public override void Interact(GameObject user)
    {
        if (user.CompareTag("Player"))
        {
            return;
        }

        base.Interact(user);
    }
}
