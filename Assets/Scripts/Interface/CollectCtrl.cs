using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCtrl : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        ICollectable iCollectable = other.transform.root.GetComponent<ICollectable>();

        if (iCollectable == null) return;

        if (!iCollectable.isCollectable) return;

        iCollectable.Collect();
    }
}
