using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillCtrl : TrainingMachineBase
{
    public string animationName;

    public override bool isInteractable {get {return true;}}

    public override void Interact()
    {
        Debug.Log($"Interaction avec le tapis de course");

        Debug.Log(PlayerController.instance.name);
        Debug.Log(trainingPosition.name);

        PlayerController.instance.transform.position = trainingPosition.position;

        PlayerController.instance.transform.rotation = trainingPosition.rotation;

        PlayerController.isTrainig = true;

        // PlayerController.instance.PlayAnimation(animationName);
    }
}
