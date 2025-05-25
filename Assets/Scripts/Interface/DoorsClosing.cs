using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsClosing : MonoBehaviour
{
    [SerializeField]
    Animator doorAnimator;

    [SerializeField]
    Collider doorCollider;

    void OnTriggerExit(Collider other)
    {
        doorAnimator.SetBool("isOpenning", false);

        doorCollider.enabled = true;
    }
}
