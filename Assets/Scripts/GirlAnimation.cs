using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;


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
    }
}
