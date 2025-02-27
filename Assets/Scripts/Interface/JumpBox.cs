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

        other.gameObject.GetComponentInChildren<Animator>().SetBool("isBoxJumping", true);

    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        other.gameObject.GetComponentInChildren<Animator>().SetBool("isBoxJumping", false);
    }

    protected override void StopTraining()
    {
        base.StopTraining();
    }

    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.jumpboxTraining += Time.deltaTime / 500;

            GameManager.instance.jumpboxTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }

    protected override IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        yield return new WaitForSeconds(trainingDuration);

        callBack();

        trainingPerson = null;
    }
}
