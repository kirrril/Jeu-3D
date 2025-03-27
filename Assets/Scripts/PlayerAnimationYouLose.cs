using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationYouLose : MonoBehaviour
{
    [SerializeField]
    private Animator animator;


    void Update()
    {
        if (PlayerControllerYouLose.instance.isGaming == false)
        {
            if (PlayerControllerYouLose.instance.isMoving)
            {
                animator.SetFloat("MovementSpeed", 2.1f);
            }

            if (PlayerControllerYouLose.instance.isMoving == false)
            {
                animator.SetFloat("MovementSpeed", 0.2f);
            }
        }
    }
}