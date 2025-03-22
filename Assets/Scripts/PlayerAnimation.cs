using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Coroutine pushCoroutine;


    void Update()
    {
        if (PlayerController.instance.isReadyToJump == false && PlayerController.instance.isTraining == false)
        {
            if (PlayerController.instance.isMoving)
            {
                animator.SetFloat("MovementSpeed", 2.1f);
            }

            if (PlayerController.instance.isMoving == false)
            {
                animator.SetFloat("MovementSpeed", 0.2f);
            }
        }

        if (PlayerController.instance.isReadyToJump)
        {
            animator.SetBool("isJumping", true);

            animator.SetFloat("JumpState", 0.5f);

            if (PlayerController.instance.isChargingJump)
            {
                animator.SetFloat("JumpState", 1.9f);
            }

            if (PlayerController.instance.isJumping)
            {
                animator.SetFloat("JumpState", 2.9f);
            }

            if (PlayerController.instance.isLanded)
            {
                animator.SetFloat("JumpState", 4.0f);
            }
        }

        if (PlayerController.instance.isReadyToJump == false)
        {
            animator.SetBool("isJumping", false);
        }

        if (PlayerController.instance.isSubmissed)
        {
            animator.SetBool("isSubmissed", true);
        }

        if (PlayerController.instance.isSubmissed == false)
        {
            animator.SetBool("isSubmissed", false);
        }

        if (PlayerController.instance.playerHasAttacked == true)
        {
            animator.SetBool("isPushing", true);

            pushCoroutine = StartCoroutine(Push());
        }

        if (PlayerController.instance.playerHasAttacked == false)
        {
            pushCoroutine = null;

            animator.SetBool("isPushing", false);
        }

        IEnumerator Push()
        {
            animator.SetFloat("PushingState", 0.5f);

            yield return new WaitForSeconds(0.375f);

            animator.SetFloat("PushingState", 1.9f);

            yield return new WaitForSeconds(0.375f);

            yield break;
        }
    }
}