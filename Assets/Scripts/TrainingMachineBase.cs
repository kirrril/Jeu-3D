using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMachineBase : MonoBehaviour//, IInteractable
{
    [SerializeField]
    public Transform trainingPosition;

    public virtual bool isInteractable => throw new System.NotImplementedException();



    public virtual void Interact()
    {
        throw new System.NotImplementedException();
    }
}
