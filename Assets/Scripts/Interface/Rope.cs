using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    // [SerializeField]
    // Transform climbingPosition;

    // [SerializeField]
    // Transform stopClimbingPosition;

    // [SerializeField]
    // GameObject player;

    // [SerializeField]
    // Animator playerAnimator;

    // Coroutine climbingCoroutine;


    // void Interact()
    // {
    //     climbingCoroutine = StartCoroutine(ClimbingPositionUpdate());
    // }


    // IEnumerator ClimbingPositionUpdate()
    // {
    //     yield return null;

    //     while (PlayerController.instance.isInClimbingZone)
    //     {
    //         if (PlayerController.instance.isClimbing)
    //         {
    //             player.GetComponent<Rigidbody>().isKinematic = true;
    //             player.transform.position = climbingPosition.position;
    //             player.transform.rotation = climbingPosition.rotation;

    //             if (PlayerController.instance.isClimbingUp)
    //             {
    //                 playerAnimator.SetBool("isClimbing", true);
    //                 playerAnimator.SetBool("isClimbingUp", true);
    //             }

    //             if (PlayerController.instance.isSlidingDown)
    //             {
    //                 playerAnimator.SetBool("isClimbing", true);
    //                 playerAnimator.SetBool("isClimbingUp", false);
    //             }
    //         }
    //         else
    //         {
    //             player.GetComponent<Rigidbody>().isKinematic = false;
    //             player.transform.position = stopClimbingPosition.position;
    //             player.transform.rotation = stopClimbingPosition.rotation;
    //             playerAnimator.SetBool("isClimbing", false);
    //             playerAnimator.SetBool("isClimbingUp", false);
    //         }

    //         yield return null;
    //     }
    // }


    // void OnTriggerEnter(Collider other)
    // {
    //     if (!other.CompareTag("Player"))
    //     {
    //         return;
    //     }

    //     Interact();
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     climbingCoroutine = null;
    // }
}
