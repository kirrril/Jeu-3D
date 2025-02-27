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
        other.gameObject.GetComponentInChildren<Animator>().SetBool("isJogging", true);

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponentInChildren<Animator>().SetBool("isJogging", false);

        base.OnTriggerExit(other);
    }

    protected override void StopTraining()
    {
        base.StopTraining();
    }

    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.treadmillTraining += Time.deltaTime / 500;

            GameManager.instance.treadmillTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }

    protected override IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        yield return new WaitForSeconds(trainingDuration);

        callBack();

        yield return new WaitForSeconds(0.5f);

        trainingPerson = null;
    }
}