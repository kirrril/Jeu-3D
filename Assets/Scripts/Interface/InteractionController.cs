using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name}");
        IInteractable iInteractable = other.transform.root.GetComponent<IInteractable>();

        if (iInteractable == null) return;

        if (!iInteractable.isInteractable) return;

        iInteractable.Interact();

    }
}
