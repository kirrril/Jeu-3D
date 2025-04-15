using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Selfie : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 3.0f;

        animationBool = "isShowingOff";
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
