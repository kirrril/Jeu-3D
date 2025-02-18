using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour, ICollectable
{
    private Spawner spawner;

    [SerializeField]
    private GameObject waterBottle;

    [SerializeField]
    private float waterContent = 1.0f;

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
        GameManager.instance.currentPlayer.water = waterContent;

        Destroy(gameObject);

        spawner.SpawnWater();
    }
}
