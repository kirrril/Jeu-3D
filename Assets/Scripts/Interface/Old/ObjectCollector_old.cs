using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectCollector_old : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // ICollectable iCollectable = other.transform.root.GetComponent<ICollectable>();

        // if (iCollectable == null) return;

        // if (!iCollectable.isCollectable) return;

        // iCollectable.Collect();

    }
}