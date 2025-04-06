using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    protected Transform climbingPosition;

    protected GameObject trainingPerson;

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
        playerAnimator.SetBool("isClimbing", true);

        yield return null;

        while (PlayerController.instance.isClimbing)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerAnimator.SetBool("isClimbingUp", true);
            }
            else
            {
                playerAnimator.SetBool("isClimbingUp", false);
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
