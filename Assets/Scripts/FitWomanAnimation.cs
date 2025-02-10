using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWomanAnimation : MonoBehaviour
{
    Animator animator;

    Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();

        lastPosition = transform.parent.position;
    }


    void Update()
    {
        Vector3 newPosition = transform.parent.position;

        if (Vector3.Distance(lastPosition, newPosition) > 0.1f)
        {
            animator.SetBool("isJogging", true);

            lastPosition = transform.parent.position;
        } else
        {
            animator.SetBool("isJogging", false);
        }
    }
}
