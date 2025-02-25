using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    private Transform jumpPosition;

    [SerializeField]
    private Transform stopJumpPosition;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerJumping();
        }
    }


    void PlayerJumping()
    {
        PlayerController.instance.transform.position = jumpPosition.position;

        PlayerController.instance.transform.rotation = jumpPosition.rotation;
    }

}

