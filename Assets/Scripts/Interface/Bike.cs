using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike : MonoBehaviour, IInteractable
{
    public GameManager gameManager;

    [SerializeField]
    private Transform cyclingPosition;

    [SerializeField]
    private Transform stopCyclingPosition;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    // private int speedGain = 20;

    private bool _isInteractable = true;

    public bool isInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCycling();
        }

    }


    void PlayerCycling()
    {
        if (!isInteractable) return;

        PlayerController.instance.transform.position = cyclingPosition.position;

        PlayerController.instance.transform.rotation = cyclingPosition.rotation;

        PlayerController.isTraining = true;

        playerAnimator.Play("Cycling");

        IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        isInteractable = false;

        IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
    }


    public void PlayerStopCycling()
    {
        // gameManager.currentPlayer.speed += speedGain;

        PlayerController.instance.transform.position = stopCyclingPosition.position;

        PlayerController.instance.transform.rotation = stopCyclingPosition.rotation;

        PlayerController.isTraining = false;

        IHM.instance.DisplayData();

        IHM.instance.stopTrainingButton.gameObject.SetActive(false);

        isInteractable = true;
    }


    void OnButtonClick()
    {
        PlayerStopCycling();
    }
}
