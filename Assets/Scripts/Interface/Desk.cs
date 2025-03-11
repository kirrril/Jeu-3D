using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [SerializeField]
    protected Transform gamingPosition;

    protected GameObject trainingPerson;

    protected string animationBool = "isGaming";

    protected Coroutine gamingCoroutine;


    public virtual void Interact(GameObject user)
    {
        trainingPerson = user;

        TakePlace();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
    }

    IEnumerator TrainingCorout()
    {
        yield return new WaitForSeconds(1f);
    }

    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }

    void TakePlace()
    {
        trainingPerson.transform.position = gamingPosition.position;
        trainingPerson.transform.rotation = gamingPosition.rotation;
    }
}