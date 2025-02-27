using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    private Transform jumpPosition;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TakePlace();
        }
    }


    void TakePlace()
    {
        PlayerController.instance.transform.position = jumpPosition.position;
        PlayerController.instance.transform.rotation = jumpPosition.rotation;
    }

}

