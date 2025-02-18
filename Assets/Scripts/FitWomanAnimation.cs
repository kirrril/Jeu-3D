using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWomanAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    FitNavMesh fitNavMesh;

    Vector3 currentPosition;

    void Start()
    {
        currentPosition = transform.parent.position;
    }


    void Update()
    {
        Vector3 newPosition = transform.parent.position;

        if (fitNavMesh.isBusy == false)
        {
            if (Vector3.Distance(currentPosition, newPosition) < 0.01f)
            {
                animator.SetBool("isWalking", false);
            }

            if (Vector3.Distance(currentPosition, newPosition) > 0.01f)
            {
                animator.SetBool("isWalking", true);
            }
        }

        currentPosition = transform.parent.position;
    }
}
