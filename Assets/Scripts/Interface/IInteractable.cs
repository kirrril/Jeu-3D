using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool isInteractable { get; set; }

    bool isInteracting { get; }

    void Interact(GameObject user);
}