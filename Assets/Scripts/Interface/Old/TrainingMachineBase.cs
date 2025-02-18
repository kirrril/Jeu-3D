using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMachineBase : MonoBehaviour, IInteractable
{
    public virtual bool isInteractable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public enum BodyPart
    {
        legs, chest, back
    }

    public BodyPart bodyPart;
}
