using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    protected Transform pushingPosition;

    protected GameObject trainingPerson;

    protected string animationBool = "isPushingDoor";

    protected Coroutine gamingCoroutine;


    public virtual void Interact(GameObject user)
    {
        trainingPerson = user;

        TakePlace();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
    }

    // IEnumerator TrainingCorout()
    // {
    //     yield return new WaitForSeconds(1f);
    // }

    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("isOpenning", true);
    }

    void OnTriggerExit(Collider other)
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("isOpenning", false);

        other.GetComponentInChildren<Animator>().SetBool(animationBool, false);
    }

    void TakePlace()
    {
        trainingPerson.transform.position = pushingPosition.position;
        trainingPerson.transform.rotation = pushingPosition.rotation;
    }
}
