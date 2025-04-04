using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Treadmill : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isJogging";
    }

    protected override void Update()
    {
        base.Update();

        TreadmillTrainingProgress();

        DisplayMachineWarning();
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.treadmillTraining == 0.35f)
        {
            IHM.instance.contextMessageCorout = StartCoroutine(MachineWarning());

            trainingAudio.Stop();
        }
    }

    public IEnumerator MachineWarning()
    {
        IHM.instance.contextMessage.text = $"TREADMILL TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"TREADMILL TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"TREADMILL TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }


    void TreadmillTrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.treadmillTraining += Time.deltaTime / 500;

            GameManager.instance.treadmillTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }
}