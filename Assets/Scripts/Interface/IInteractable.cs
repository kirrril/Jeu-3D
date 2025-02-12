using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool isInteractable { get; }

    // void TakePlace();
    void Interact();
}