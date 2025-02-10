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
        Debug.Log($"Collision avec {other.gameObject.name}");

        if (other.CompareTag("Protein"))
        {
            Debug.Log(other.gameObject.name);
            ICollectable proteinTub = other.GetComponent<ICollectable>(); /*transform.root.*/

            if (proteinTub == null) return;

            if (!proteinTub.isCollectable) return;

            proteinTub.Collect();
        }
    }
}