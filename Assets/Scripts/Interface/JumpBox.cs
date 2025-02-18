using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
[SerializeField]
    FitNavMesh fitNavMesh;

    [SerializeField]
    private Transform boxJumpingPosition;

    [SerializeField]
    private Transform stopBoxJumpingPosition;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator fitWomanAnimator;

    [SerializeField]
    private Transform fitWomanTransform;


    private bool _isInteractable = true;

    public bool isInteractable
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
            PlayerBoxJumping();
        }

        if (other.CompareTag("FitWoman"))
        {
            FitBoxJumping();
        }

    }


    void PlayerBoxJumping()
    {
        if (!isInteractable) return;

        PlayerController.instance.transform.position = boxJumpingPosition.position;

        PlayerController.instance.transform.rotation = boxJumpingPosition.rotation;

        PlayerController.isTraining = true;

        playerAnimator.Play("BoxJumping");

        isInteractable = false;

        IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
    }


    public void PlayerStopBoxJumping()
    {
        PlayerController.instance.transform.position = stopBoxJumpingPosition.position;

        PlayerController.instance.transform.rotation = stopBoxJumpingPosition.rotation;

        PlayerController.isTraining = false;

        IHM.instance.stopTrainingButton.gameObject.SetActive(false);

        isInteractable = true;
    }


    void FitBoxJumping()
    {
        if (!isInteractable) return;

        fitWomanTransform.position = boxJumpingPosition.position;

        fitWomanTransform.rotation = boxJumpingPosition.rotation;

        if (fitNavMesh.isBusy == true)
        {
            // fitWomanAnimator.Play("Jogging");
        }

        isInteractable = false;
    }


    void OnButtonClick()
    {
        PlayerStopBoxJumping();
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
