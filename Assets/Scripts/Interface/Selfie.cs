using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfie : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isMakingSelfie";
    }
}
