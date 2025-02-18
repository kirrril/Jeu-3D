using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;


    void Update()
    {
        if (PlayerController.isTraining == false && PlayerController.isReadyToJump == false)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                animator.Play("Walking");
            }
            else
            {
                animator.Play("Idle");
            }
        }

        if (PlayerController.isReadyToJump)
        {
            Debug.Log("isReadyToJump" + PlayerController.isReadyToJump);

            if (Input.GetKey(KeyCode.Space))
            {
                animator.Play("ChargingJump");
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.Play("Jump");
            }
        }

        // if (PlayerController.instance.isLanding)
        // {
        //     Debug.Log("PlayerController.instance.isLanding" + PlayerController.instance.isLanding);
        //     animator.Play("Landing");
        // }
    }
}