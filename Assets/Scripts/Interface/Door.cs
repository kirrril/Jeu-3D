using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Collider doorCollider;

    [SerializeField]
    Animator doorAnimator;

    protected GameObject trainingPerson;

    protected string animationBool = "isPushing";

    protected Coroutine pushingCoroutine;


    void Interact(GameObject user)
    {
        if (!user.CompareTag("Player")) return;

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

            PlayerController.instance.voiceHa.Play();

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
                else if (GameManager.instance.currentPlayer.chestTraining >= 1f)
                {
                    doorCollider.enabled = false;

                    doorAnimator.SetBool("isOpenning", true);

                    playerAnimator.SetFloat("PushingState", 1.9f);

                    yield return new WaitForSeconds(0.375f);

                    playerAnimator.SetBool(animationBool, false);

                    yield break;
                }
            }

        }
    }


    void OnTriggerEnter(Collider other)
    {
        PlayerController.instance.isPushingTheDoor = true;

        Interact(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerController.instance.isPushingTheDoor = false;

        // doorAnimator.SetBool("isOpenning", false);

        // Animator playerAnimator = other.GetComponentInChildren<Animator>();
        // playerAnimator.SetBool(animationBool, false);

        StopCoroutine(pushingCoroutine);

        pushingCoroutine = null;

        // doorCollider.enabled = true;
    }
}
