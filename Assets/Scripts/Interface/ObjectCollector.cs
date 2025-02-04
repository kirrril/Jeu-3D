using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        ICollectable icollectable = other.transform.root.GetComponent<ICollectable>();

        if (icollectable == null) return;

        if (!icollectable.isCollectable) return;

        icollectable.Collect();
    }


}