using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCtrl : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        IInteractable iInteractable = other.transform.root.GetComponent<IInteractable>();

        if (iInteractable == null) return;

        if (!iInteractable.isInteractable) return;

        iInteractable.Interact();
    }
}