using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinCan : MonoBehaviour, ICollectable
{
    [SerializeField]
    private int proteinAmount = 5;
    
    public bool isCollectable {get {return true;}}

    public void Collect()
    {
        Debug.Log($"{proteinAmount} proteins collected");
    }

}
