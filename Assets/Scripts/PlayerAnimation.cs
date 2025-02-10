using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Start()
    {

    }


    void Update()
    {
        if (PlayerController.isTrainig == false)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }

        if (PlayerController.isTrainig == true)
        {
            animator.SetBool("isWalking", false);
            
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                animator.SetBool("isJogging", true);
            }
            else
            {
                animator.SetBool("isJogging", false);
            }
        }
    }

}
