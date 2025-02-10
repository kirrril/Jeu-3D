using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscularAnimation : MonoBehaviour
{
    Animator animator;

    Vector3 currentPosition;
    Vector3 newPosition;


    void Start()
    {
        animator = GetComponent<Animator>();

        currentPosition = transform.parent.position;
    }

    void Update()
    {
        newPosition = transform.parent.position;

        if (Vector3.Distance(currentPosition,newPosition) > 0.5f)
        {
            animator.SetBool("isWalking", true);

            currentPosition = transform.parent.position;
        } else
        {
            animator.SetBool("isWalking", false);
        }

        if (MuscularNavMesh.showMuscles == true)
        {
            animator.SetBool("isShowingMuscles", true);
            
        } else
        {
            animator.SetBool("isShowingMuscles", false);
        }
    }
}
