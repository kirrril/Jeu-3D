using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform jumpPosition;

    [SerializeField]
    private Transform stopJumpPosition;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator trampolineAnimator;

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
            PlayerJumping();
        }
    }


    void PlayerJumping()
    {
        PlayerController.instance.transform.position = jumpPosition.position;

        PlayerController.instance.transform.rotation = jumpPosition.rotation;

        PlayerController.instance.isGrounded = true;

        PlayerController.isReadyToJump = true;

        playerAnimator.Play("Idle");

        Debug.Log(PlayerController.isReadyToJump);

        // IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        // IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
    }

    void PlayerStopJumping()
    {
        PlayerController.instance.transform.position = stopJumpPosition.position;

        PlayerController.instance.transform.rotation = stopJumpPosition.rotation;

        PlayerController.isReadyToJump = false;

        // IHM.instance.stopTrainingButton.gameObject.SetActive(false);
    }

    void OnButtonClick()
    {
        PlayerStopJumping();
    }

}

