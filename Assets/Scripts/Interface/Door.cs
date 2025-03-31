using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    protected Transform pushingPosition;

    [SerializeField]
    Collider doorCollider;

    protected GameObject trainingPerson;

    protected string animationBool = "isPushing";

    protected Coroutine pushingCoroutine;


    void Interact(GameObject user)
    {
        pushingCoroutine = StartCoroutine(PushingCorout(user));
    }


    IEnumerator PushingCorout(GameObject user)
    {
        while (true)
        {
            while (!Input.GetKey(KeyCode.Space))
            {
                yield return null;
            }

            Animator playerAnimator = user.GetComponentInChildren<Animator>();
            playerAnimator.SetBool(animationBool, true);
            playerAnimator.SetFloat("PushingState", 0.5f);

            while (Input.GetKey(KeyCode.Space))
            {
                yield return null;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (GameManager.instance.currentPlayer.chestTraining < 1f)
                {
                    playerAnimator.SetBool(animationBool, false);
                }
                else
                {
                    doorCollider.enabled = false;

                    PlayerController.instance.voiceHa.Play();

                    Animator doorAnimator = GetComponentInChildren<Animator>();
                    doorAnimator.SetBool("isOpenning", true);

                    playerAnimator.SetFloat("PushingState", 1.9f);

                    yield return new WaitForSeconds(0.375f);

                    playerAnimator.SetBool(animationBool, false);

                    pushingCoroutine = null;

                    yield break;
                }


            }

        }
    }


    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Animator doorAnimator = GetComponentInChildren<Animator>();
        doorAnimator.SetBool("isOpenning", false);

        Animator playerAnimator = other.GetComponentInChildren<Animator>();
        playerAnimator.SetBool(animationBool, false);

        StopCoroutine(PushingCorout(other.gameObject));

        doorCollider.enabled = true;
    }
}
