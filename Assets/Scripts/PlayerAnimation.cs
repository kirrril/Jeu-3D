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
        // if (PlayerController.instance.isReadyToJump == false && PlayerController.instance.isTraining == false)
        // {
        if (PlayerController.instance.isMoving)
        {
            animator.SetFloat("MovementSpeed", 2.1f);
        }

        if (PlayerController.instance.isMoving == false)
        {
            animator.SetFloat("MovementSpeed", 0.2f);
        }
        // }


        if (PlayerController.instance.isInJumpZone)
        {
            if (PlayerController.instance.isMoving)
            {
                animator.SetBool("isJumping", false);
                animator.SetFloat("MovementSpeed", 2.1f);
            }

            if (PlayerController.instance.isMoving == false)
            {
                animator.SetFloat("MovementSpeed", 0.2f);
                animator.SetBool("isJumping", true);
            }
        }



        if (PlayerController.instance.isLanding)
        {
            animator.SetBool("isLanding", true);
            animator.SetBool("isJumping", false);
        }

        if (!PlayerController.instance.isLanding)
        {
            animator.SetBool("isLanding", false);
        }

        if (PlayerController.instance.hasFallen)
        {
            animator.SetBool("isLanding", false);
            animator.SetBool("isJumping", false);
        }

        if (PlayerController.instance.isJumping)
        {
            animator.SetFloat("JumpState", 2.9f);
        }
        else if (PlayerController.instance.isChargingJump)
        {
            animator.SetFloat("JumpState", 1.9f);
        }
        else if (PlayerController.instance.isReadyToJump && !PlayerController.instance.isMoving)
        {
            animator.SetBool("isJumping", true);

            animator.SetFloat("JumpState", 0f);
        }



        if (PlayerController.instance.isSubmissed)
        {
            animator.SetBool("isSubmissed", true);
        }

        if (PlayerController.instance.isSubmissed == false)
        {
            animator.SetBool("isSubmissed", false);
        }

        if (PlayerController.instance.playerAttacks == true)
        {
            animator.SetBool("isPushing", true);

            pushCoroutine = StartCoroutine(Push());
        }


        IEnumerator Push()
        {
            animator.SetFloat("PushingState", 0.5f);

            yield return new WaitForSeconds(0.3f);

            animator.SetFloat("PushingState", 1.9f);

            yield return new WaitForSeconds(0.3f);

            animator.SetFloat("PushingState", 0.1f);

            animator.SetBool("isPushing", false);

            pushCoroutine = null;

            yield break;
        }


        // if (PlayerController.instance.isInClimbingZone)
        // {
        // if (PlayerController.instance.isClimbing)
        // {
        //     animator.SetBool("isClimbing", true);

        //     if (PlayerController.instance.isClimbingUp)
        //     {
        //         animator.SetBool("isClimbingUp", true);
        //     }
        //     else
        //     {
        //         animator.SetBool("isClimbingUp", false);
        //     }
        // }
        // else
        // {
        //     animator.SetBool("isClimbingUp", false);
        //     animator.SetBool("isClimbing", false);
        // }
        // }

        if (PlayerController.instance.isClimbing && !PlayerController.instance.isClimbingUp)
        {
            animator.SetBool("isClimbing", true);
            animator.SetBool("isClimbingUp", false);
        }

        if (PlayerController.instance.isClimbing && PlayerController.instance.isClimbingUp)
        {
            animator.SetBool("isClimbing", true);
            animator.SetBool("isClimbingUp", true);
        }

        if (!PlayerController.instance.isClimbing && !PlayerController.instance.isClimbingUp)
        {
            animator.SetBool("isClimbing", false);
            animator.SetBool("isClimbingUp", false);
        }
    }
}