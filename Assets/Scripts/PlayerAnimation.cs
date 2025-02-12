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

        


    }

}
