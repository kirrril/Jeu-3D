using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour, IInteractable
{
    public GameManager gameManager;

    [SerializeField]
    public Transform trainingPosition;

    [SerializeField]
    public Transform stopTrainingPosition;

    [SerializeField]
    public Animator playerAnimator;

    [SerializeField]
    private int speedGain = 20;

    public bool isInteractable { get { return true; } }

    public void Interact()
    {
        gameManager.currentPlayer.speed += speedGain;


        PlayerController.instance.transform.position = trainingPosition.position;

        PlayerController.instance.transform.rotation = trainingPosition.rotation;

        PlayerController.isTrainig = true;

        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            playerAnimator.Play("TreadmillRun");
        }

        if (Input.GetKey(KeyCode.Keypad0))
        {
            PlayerController.instance.transform.position = stopTrainingPosition.position;

            PlayerController.instance.transform.rotation = stopTrainingPosition.rotation;

            PlayerController.isTrainig = false;
        }

    }
}
