using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation instance;

    [SerializeField]
    private Animator animator;

    public bool treadmillTraining;
    public bool bikeTraining;
    public bool jumpboxTraining;


    void Awake()
    {
        instance = this;
    }


    void Update()
    {             
        if (PlayerController.instance.isMoving)
        {
            animator.SetFloat("MovementSpeed", 2.1f);
        }

        if (PlayerController.instance.isMoving == false)
        {
            animator.SetFloat("MovementSpeed", 0.2f);
        }


        if (treadmillTraining)
        {
            animator.SetBool("isJogging", true);
        }

        if (!treadmillTraining)
        {
            animator.SetBool("isJogging", false);
        }


        if (bikeTraining)
        {
            animator.SetBool("isCycling", true);
        }

        if (!bikeTraining)
        {
            animator.SetBool("isCycling", false);
        }


        if (jumpboxTraining)
        {
            animator.SetBool("isBoxJumping", true);
        }

        if (!jumpboxTraining)
        {
            animator.SetBool("isBoxJumping", false);
        }


        if (PlayerController.instance.isReadyToJump)
        {
            animator.SetBool("isJumping", true);

            if (PlayerController.instance.chargeJump < 0.1f)
            {
                animator.SetFloat("JumpState", 0.2f);
            }

            if (PlayerController.instance.chargeJump > 1.8f)
            {
                animator.SetFloat("JumpState", 1.5f);
            }
        }


        if (PlayerController.instance.isJumping)
        {
            animator.SetFloat("JumpState", 2.8f);
        }


        if (PlayerController.instance.isLanded)
        {
            animator.SetFloat("JumpState", 3.9f);
            
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isJumping", false);
    }
}