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

        other.gameObject.GetComponentInChildren<Animator>().SetBool("isCycling", true);

        Debug.Log($"other.gameObject.name {other.gameObject.name}");

    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        other.gameObject.GetComponentInChildren<Animator>().SetBool("isCycling", false);
    }

    protected override void StopTraining()
    {
        base.StopTraining();
    }

    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 500;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }

    protected override IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        yield return new WaitForSeconds(trainingDuration);

        callBack();

        trainingPerson = null;
    }
}
