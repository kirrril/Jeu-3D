using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinTub : MonoBehaviour, ICollectable
{
    private Spawner spawner;

    [SerializeField]
    private GameObject proteinTub;

    public bool isCollectable { get { return true; } }


    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        GameManager.instance.currentPlayer.protein += 1;

        Destroy(gameObject);

        spawner.SpawnItem("Protein");
    }
}

