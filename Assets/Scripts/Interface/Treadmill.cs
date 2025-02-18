using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Treadmill : TrainingMachineBase, IInteractable
{
    [SerializeField]
    FitNavMesh fitNavMesh;

    [SerializeField]
    private Transform runningPosition;

    [SerializeField]
    private Transform stopRunningPosition;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator fitWomanAnimator;

    [SerializeField]
    private Transform fitWomanTransform;


    private bool _isInteractable = true;

    public override bool isInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }


    void Update()
    {
        if (PlayerController.isTraining)
        {
            LegsTrainingProgress();

            WaterLevel();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRunning();
        }

        if (other.CompareTag("FitWoman"))
        {
            FitWomanRunning();
        }

    }


    void PlayerRunning()
    {
        if (!isInteractable) return;

        PlayerController.instance.transform.position = runningPosition.position;

        PlayerController.instance.transform.rotation = runningPosition.rotation;

        PlayerController.isTraining = true;

        playerAnimator.Play("Running");

        IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        isInteractable = false;

        IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
    }


    public void PlayerStopRunning()
    {
        PlayerController.instance.transform.position = stopRunningPosition.position;

        PlayerController.instance.transform.rotation = stopRunningPosition.rotation;

        PlayerController.isTraining = false;

        IHM.instance.stopTrainingButton.gameObject.SetActive(false);

        isInteractable = true;
    }


    void FitWomanRunning()
    {
        if (!isInteractable) return;

        fitWomanTransform.position = runningPosition.position;

        fitWomanTransform.rotation = runningPosition.rotation;

        if (fitNavMesh.isBusy == true)
        {
            fitWomanAnimator.Play("Jogging");
        }

        isInteractable = false;
    }


    void OnButtonClick()
    {
        PlayerStopRunning();
    }

    void LegsTrainingProgress()
    {
        GameManager.instance.currentPlayer.legsTraining += Time.deltaTime / 100;

        GameManager.instance.currentPlayer.legsTraining = Mathf.Clamp(GameManager.instance.currentPlayer.legsTraining, 0, 0.35f);
    }

    void WaterLevel()
    {
        float waterLoss = Time.deltaTime / 100;
        GameManager.instance.currentPlayer.water -= waterLoss;
    }
}
