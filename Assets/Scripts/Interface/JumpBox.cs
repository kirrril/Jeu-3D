using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class JumpBox : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();
        
        trainingDuration = 10.0f;

        animationBool = "isBoxJumping";
    }

    protected override void Update()
    {
        base.Update();

        JumpboxTrainingProgress();
    }

        public void DisplayMachineWarning()
    {
        if (GameManager.instance.jumpboxTraining == 0.35f)
        {
            IHM.instance.contextMessageCorout = StartCoroutine(MachineWarning());

            trainingAudio.Stop();
        }
    }

    public IEnumerator MachineWarning()
    {
        IHM.instance.contextMessage.text = $"JUMPBOX TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"JUMPBOX TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = $"JUMPBOX TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    void JumpboxTrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.jumpboxTraining += Time.deltaTime / 500;

            GameManager.instance.jumpboxTraining = Mathf.Clamp(GameManager.instance.jumpboxTraining, 0, 0.35f);

            DisplayMachineWarning();
        }
    }
}
