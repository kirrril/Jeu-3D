using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    protected Transform climbingPosition;

    protected GameObject trainingPerson;

    protected string animationBool = "isClimbing";

    Coroutine climbingCoroutine;


    void Interact(GameObject user)
    {
        trainingPerson = user;

        TakePlace();

        climbingCoroutine = StartCoroutine(ClimbingCorout(trainingPerson));
    }


    IEnumerator ClimbingCorout(GameObject user)
    {
        Animator playerAnimator = user.GetComponentInChildren<Animator>();
        playerAnimator.SetBool(animationBool, true);

        while (PlayerController.instance.isClimbing)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerAnimator.SetFloat("ClimbingState", 1.9f);
            }
            else
            {
                playerAnimator.SetFloat("ClimbingState", 0.1f);
            }

            yield return null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        trainingPerson = other.gameObject;

        TakePlace();

        Interact(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        trainingPerson = null;

        climbingCoroutine = null;
    }


    void TakePlace()
    {
        trainingPerson.GetComponent<Rigidbody>().isKinematic = true;
        trainingPerson.transform.position = climbingPosition.position;
        trainingPerson.transform.rotation = climbingPosition.rotation;
    }
}
