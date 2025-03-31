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

    [SerializeField]
    AudioSource waterCollect;

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

        if (other.CompareTag("Ground"))
        {
            StartCoroutine(Expire());
        }
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);

        spawner.SpawnItem("Water");
    }

    public void Collect()
    {
        PlayerController.instance.sfxWaterCollect.Play();
        
        GameManager.instance.currentPlayer.water = waterContent;

        Destroy(gameObject);

        spawner.SpawnItem("Water");
    }
}
