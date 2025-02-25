using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfie : MonoBehaviour, IInteractable
{
    public virtual bool isInteractable { get; set; } = true;
    
    [SerializeField]
    private Transform trainingPosition;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl"))
        {
            other.transform.position = trainingPosition.position;

            GirlAnimation.instance.makingSelfie = true;

            isInteractable = false;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Girl"))
        {
            GirlAnimation.instance.makingSelfie = false;

            isInteractable = true;
        }
    }
}
